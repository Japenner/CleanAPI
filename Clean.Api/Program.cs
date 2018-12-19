namespace Mosaic.Api
{
    using System;
    using System.IO;
    using Clean.Web;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Newtonsoft.Json;
    using Serilog;

    /// <summary>
    /// Entry class of executable.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point for application.
        /// </summary>
        /// <param name="args">The command line parameters.</param>
        /// <returns>A numeric exit code.</returns>
        public static int Main(string[] args)
        {
            Console.Title = "Clean API";
            var settings = GetSettings();

            try
            {
                CreateWebHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Gets the environment setting out of the appsettings file
        /// </summary>
        /// <returns>the environment setting</returns>
        private static string GetSettings()
        {
            var env = "undefined";
            using (StreamReader file = new StreamReader("appsettings.json"))
            {
                string json = file.ReadToEnd();
                var settings = JsonConvert.DeserializeAnonymousType(json, new { Site = string.Empty, Slack = new { ErrorWebhookUri = string.Empty } });
                env = settings.Site;
            }

            return env;
        }

        /// <summary>
        /// Creates the web host configuration builder.
        /// </summary>
        /// <param name="args">The command line parameters.</param>
        /// <returns>Returns the web host builder.</returns>
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(o => { o.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10); })
                .UseUrls("http://localhost:5001")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
