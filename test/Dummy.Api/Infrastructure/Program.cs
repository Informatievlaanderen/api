namespace Dummy.Api.Infrastructure
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.Api;
    using Microsoft.Extensions.Hosting;
    using Modules;

    public static class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args)
            => new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(cb =>
                {
                    cb.RegisterModule(new ApiModule());
                }))
                .UseDefaultForApi<Startup>(new ProgramOptions
                {
                    Hosting =
                    {
                        HttpPort = 8000
                    },
                    Logging =
                    {
                        WriteTextToConsole = false,
                        WriteJsonToConsole = true
                    },
                    Runtime =
                    {
                        CommandLineArgs = args
                    }
                });
    }
}
