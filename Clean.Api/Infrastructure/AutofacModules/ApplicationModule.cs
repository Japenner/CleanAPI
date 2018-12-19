namespace Clean.Api.Infrastructure.AutofacModules
{
    using Autofac;
    using Clean.Core.Interfaces;
    using Clean.Infrastructure.ModelBuilders;
    using Clean.Infrastructure.Repositories;
    using Clean.Web;
    using FluentValidation;
    using System.Linq;

    /// <summary>
    /// Configures the application for Autofac.
    /// </summary>
    public class ApplicationModule : Module
    {
        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="builder">The builder to use.</param>
        protected override void Load(ContainerBuilder builder)
        {
            var coreAssembly = typeof(IToDoItemsRepository).Assembly;
            var infrastructureAssembly = typeof(ToDoItemsRepository).Assembly;
            var coreApiAssembly = typeof(Startup).Assembly;

            builder.RegisterAssemblyTypes(coreAssembly, infrastructureAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(coreApiAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            // builder
            //    .RegisterAssemblyTypes(coreAssembly)
            //    .Where(t => t.BaseType == typeof(ToDoItemFilterBaseSpecification))
            //    .As<ToDoItemFilterBaseSpecification>();
            builder
                .RegisterAssemblyTypes(infrastructureAssembly)
                .Where(t => t.IsAssignableTo<IEntityConfiguration>())
                .AsImplementedInterfaces();
        }
    }
}
