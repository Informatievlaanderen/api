namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using AspNetCore.Mvc.Formatters.Json;
    using Destructurama;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Serilog;
    using Serilog.Debugging;

    public static class ProgramDefaults
    {
        public static IWebHostBuilder UseDefaultForApi<T>(
            this IWebHostBuilder hostBuilder,
            int httpPort = 5000,
            int? httpsPort = null,
            Func<X509Certificate2> httpsCertificate = null,
            string[] commandLineArgs = null) where T : class
        {
            SelfLog.Enable(Console.WriteLine);

            commandLineArgs = PatchRiderBug<T>(commandLineArgs);

            ConfigureEncoding();
            ConfigureJsonSerializerSettings();
            ConfigureAppDomainExceptions();

            var environment = hostBuilder.GetSetting("environment");

            return hostBuilder
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;

                    if (environment == "Development")
                        AddDevelopmentPorts(options, httpPort, httpsPort, httpsCertificate?.Invoke());
                })
                .UseLibuv()
                .CaptureStartupErrors(true)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseWebRoot("wwwroot")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config
                        .SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"appsettings.{env.EnvironmentName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
                        .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
                        .AddEnvironmentVariables()
                        .AddCommandLine(commandLineArgs ?? new string[0]);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    var loggerConfiguration = new LoggerConfiguration()
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .WriteTo.Console()
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithThreadId()
                        .Enrich.WithEnvironmentUserName()
                        .Destructure.JsonNetTypes();

                    var logger = Log.Logger = loggerConfiguration.CreateLogger();

                    logging.AddSerilog(logger);
                })
                .UseStartup<T>();
        }

        private static string[] PatchRiderBug<T>(string[] commandLineArgs) where T : class
        {
            // Note: Rider starts debugging with
            // <source location>/src/.../bin/Debug/netcoreapp2.1/win10-x64/....exe
            // No idea why, but if we pass this below to .AddCommandLine(args)
            // the ConfigBuilder crashes because it does not recognize
            // that as a valid config argument. The 2 lines below prevent that.
            if (commandLineArgs != null &&
                commandLineArgs.Any() &&
                commandLineArgs[0].EndsWith($"{Path.GetFileNameWithoutExtension(typeof(T).Assembly.CodeBase)}.exe"))
                commandLineArgs = commandLineArgs.Skip(1).ToArray();

            return commandLineArgs;
        }

        private static void ConfigureEncoding()
            => Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        private static void ConfigureJsonSerializerSettings()
        {
            var jsonSerializerSettings = JsonSerializerSettingsProvider.CreateSerializerSettings().ConfigureDefaultForApi();
            JsonConvert.DefaultSettings = () => jsonSerializerSettings;
        }

        private static void ConfigureAppDomainExceptions()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
                Log.Debug(
                    eventArgs.Exception,
                    "FirstChanceException event raised in {AppDomain}.",
                    AppDomain.CurrentDomain.FriendlyName);

            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                Log.Fatal(
                    (Exception) eventArgs.ExceptionObject,
                    "Encountered a fatal exception, exiting program.");
        }

        private static void AddDevelopmentPorts(
            KestrelServerOptions options,
            int httpPort,
            int? httpsPort,
            X509Certificate2 certificate)
        {
            AddListener(options, httpPort, null);

            if (httpsPort.HasValue && certificate != null)
                AddListener(options, httpsPort.Value, certificate);
        }

        private static void AddListener(
            KestrelServerOptions options,
            int port,
            X509Certificate2 certificate)
        {
            options.Listen(
                new IPEndPoint(IPAddress.Loopback, port),
                listenOptions =>
                {
                    listenOptions.UseConnectionLogging();

                    if (null != certificate)
                        listenOptions.UseHttps(certificate);
                });
        }
    }
}
