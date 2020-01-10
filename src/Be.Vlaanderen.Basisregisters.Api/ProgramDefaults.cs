namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using Amazon;
    using AspNetCore.Mvc.Formatters.Json;
    using Aws.DistributedMutex;
    using Destructurama;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Serilog;
    using Serilog.Debugging;
    using Serilog.Formatting.Compact;

    public class DevelopmentCertificate
    {
        public string Name { get; }
        public string Key { get; }

        public DevelopmentCertificate(string name, string key)
        {
            Name = name;
            Key = key;
        }

        public X509Certificate2 ToCertificate() => new X509Certificate2(Name, Key);
    }

    public class ProgramOptions
    {
        public HostingOptions Hosting { get; } = new HostingOptions();

        public class HostingOptions
        {
            public int? HttpPort { get; set; } = null;
            public int? HttpsPort { get; set; } = null;
            public Func<X509Certificate2>? HttpsCertificate { get; set; }
        }

        public LoggingOptions Logging { get; } = new LoggingOptions();

        public class LoggingOptions
        {
            public bool WriteTextToConsole { get; set; } = true;
            public bool WriteJsonToConsole { get; set; } = false;
        }

        public RuntimeOptions Runtime { get; } = new RuntimeOptions();

        public class RuntimeOptions
        {
            public string[]? CommandLineArgs { get; set; }
        }

        public MiddlewareHookOptions MiddlewareHooks { get; } = new MiddlewareHookOptions();

        public class MiddlewareHookOptions
        {
            public Action<WebHostBuilderContext, IConfigurationBuilder>? ConfigureAppConfiguration { get; set; }
            public Action<WebHostBuilderContext, LoggerConfiguration>? ConfigureSerilog { get; set; }
            public Action<WebHostBuilderContext, ILoggingBuilder>? ConfigureLogging { get; set; }

            public Func<IConfiguration, DistributedLockOptions>? ConfigureDistributedLock { get; set; }
        }
    }

    public static class ProgramDefaults
    {
        public static IWebHostBuilder UseDefaultForApi<T>(
            this IWebHostBuilder hostBuilder,
            ProgramOptions options) where T : class
        {
            SelfLog.Enable(Console.WriteLine);

            options.Runtime.CommandLineArgs = PatchRiderBug<T>(options.Runtime.CommandLineArgs);

            ConfigureEncoding();
            ConfigureJsonSerializerSettings();
            ConfigureAppDomainExceptions();

            var environment = hostBuilder.GetSetting("environment");

            return hostBuilder
                .UseKestrel(x =>
                {
                    x.AddServerHeader = false;

                    // Needs to be bigger than traefik timeout, which is 90seconds
                    // https://github.com/containous/traefik/issues/3237
                    x.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(120);

                    if (environment == "Development")
                        AddDevelopmentPorts(
                            x,
                            options.Hosting.HttpPort,
                            options.Hosting.HttpsPort,
                            options.Hosting.HttpsCertificate?.Invoke());
                })
                .UseSockets()
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
                        .AddCommandLine(options.Runtime.CommandLineArgs ?? new string[0]);

                    options.MiddlewareHooks.ConfigureAppConfiguration?.Invoke(hostingContext, config);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    var loggerConfiguration = new LoggerConfiguration()
                        .ReadFrom.Configuration(hostingContext.Configuration);

                    if (options.Logging.WriteTextToConsole)
                        loggerConfiguration = loggerConfiguration.WriteTo.Console();

                    if (options.Logging.WriteJsonToConsole)
                        loggerConfiguration = loggerConfiguration.WriteTo.Console(new RenderedCompactJsonFormatter());

                    loggerConfiguration = loggerConfiguration
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithThreadId()
                        .Enrich.WithEnvironmentUserName()
                        .Destructure.JsonNetTypes();

                    options.MiddlewareHooks.ConfigureSerilog?.Invoke(hostingContext, loggerConfiguration);

                    var logger = Log.Logger = loggerConfiguration.CreateLogger();

                    logging.AddSerilog(logger);

                    options.MiddlewareHooks.ConfigureLogging?.Invoke(hostingContext, logging);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddSingleton(options);
                })
                .UseStartup<T>();
        }

        public static void RunWithLock<T>(this IWebHostBuilder webHostBuilder) where T : class
        {
            var webHost = webHostBuilder.Build();
            var services = webHost.Services;
            var logger = services.GetService<ILogger<T>>();
            var options = services.GetService<ProgramOptions>();
            var configuration = services.GetService<IConfiguration>();

            var distributedLockOptions = options
                .MiddlewareHooks
                .ConfigureDistributedLock
                ?.Invoke(configuration);

            DistributedLock<T>.Run(
                () => webHost.Run(),
                distributedLockOptions ?? DistributedLockOptions.Defaults,
                logger);
        }

        private static string[]? PatchRiderBug<T>(string[]? commandLineArgs) where T : class
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
            int? httpPort,
            int? httpsPort,
            X509Certificate2? certificate)
        {
            if (httpPort.HasValue)
                AddListener(options, httpPort.Value, null);

            if (httpsPort.HasValue && certificate != null)
                AddListener(options, httpsPort.Value, certificate);
        }

        private static void AddListener(
            KestrelServerOptions options,
            int port,
            X509Certificate2? certificate)
        {
            options.Listen(
                new IPEndPoint(IPAddress.Loopback, port),
                listenOptions =>
                {
                    listenOptions.UseConnectionLogging();

                    if (certificate == null)
                        return;

                    listenOptions.UseHttps(certificate);
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });
        }
    }
}
