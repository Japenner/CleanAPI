namespace Clean.Web
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Clean.Api.Infrastructure.AutofacModules;
    using Clean.Infrastructure;
    using Clean.Infrastructure.Repositories;
    using Clean.Infrastructure.Services;
    using Clean.Web.Application.Behaviors;
    using Clean.Web.Application.Infrastructure;
    using Clean.Web.Application.Infrastructure.Filters;
    using Clean.Web.Infrastructure.Filters;
    using CleanArchictecture.Web.Infrastructure.Swagger;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;
    using Mosaic.Api.Controllers;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Startup configures the web server.
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">Environment information.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            environment = env;
        }

        /// <summary>
        /// Gets the current configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to add registered services to.</param>
        /// <returns>The configured service provider.</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(opts => { opts.ResourcesPath = "Application\\Translations"; });

            ConfigureMediatr(services);
            ConfigureMvc(services);
            ConfigureSwagger(services);

            services.AddOptions();
            services.Configure<CloudinaryConfigSettings>(Configuration.GetSection("Cloudinary"));

            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new ApplicationModule());

            var provider = new AutofacServiceProvider(container.Build());
            return provider;
        }

        /// <summary>
        /// Configures the application pipeline.
        /// </summary>
        /// <param name="app">Application information.</param>
        /// <param name="env">Environment information</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<CleanContext>();
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    context.Database.Migrate();
                }
            }

            app.UseCors("AllowAllHeaders");
            app.UseAuthentication();

            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
            };

            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };

            app.UseRequestLocalization(options);
            app.UseRewriter(new RewriteOptions().AddRedirect(@"^$", "/swagger/"));
            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                var name = Configuration.GetValue<string>("Site");
                name = char.ToUpper(name[0]) + name.Substring(1);

                c.DocumentTitle = $"{name} - Clean API - Swagger UI";

                c.SwaggerEndpoint("/swagger/1/swagger.json", "Clean API v1");
            });
        }

        private static string GetXmlCommentsForAssembly(Assembly assembly)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }

        private void ConfigureMediatr(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(ToDoItemsController));
            services.AddMediatR(typeof(ToDoItemsRepository));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        }

        private void ConfigureMvc(IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped((serviceProvider) =>
            {
                var factory = serviceProvider.GetService<IUrlHelperFactory>();
                var contextAccessor = serviceProvider.GetService<IActionContextAccessor>();
                return factory.GetUrlHelper(contextAccessor.ActionContext);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Content-Disposition");
                });
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<HttpGlobalExceptionFilter>();
                options.Filters.Add<ModelStateValidationFilter>();
                options.Filters.Add<NoCacheFilter>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddControllersAsServices();

            services.AddApiVersioning(options =>
            {
                options.Conventions.Controller<ToDoItemsController>().IsApiVersionNeutral();
            });

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<CleanContext>(
                options =>
                    options.UseSqlServer(Configuration.GetConnectionString("CleanDatabase"), b => b.MigrationsAssembly("Clean.Api")));
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("1", new Info { Title = "Clean API", Version = "v1" });
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    var versions = apiDesc.ControllerAttributes()
#pragma warning restore CS0618 // Type or member is obsolete
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"{v.ToString()}" == docName);
                });

                c.OperationFilter<VersionRemovalOperationFilter>();
                c.OperationFilter<FileUploadOperationFilter>();
                c.DocumentFilter<PathVersionDocumentFilter>();
                c.IncludeXmlComments(GetXmlCommentsForAssembly(typeof(Startup).Assembly));
                c.IncludeXmlComments(GetXmlCommentsForAssembly(typeof(ToDoItemsController).Assembly));
                c.IncludeXmlComments(GetXmlCommentsForAssembly(typeof(ToDoItemsRepository).Assembly));
                c.DescribeAllEnumsAsStrings();
            });
        }
    }
}
