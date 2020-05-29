namespace Dummy.Api.Infrastructure
{
    using Microsoft.AspNetCore.Hosting;
    using Be.Vlaanderen.Basisregisters.Api;

    public class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => new WebHostBuilder()
                .UseDefaultForApi<Startup>(
                    new ProgramOptions
                    {
                        Hosting =
                        {
                            HttpPort = 8000,
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
