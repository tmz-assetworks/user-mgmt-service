using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

using UsersService.Api;

namespace UsersService.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //     //.Enrich.FromLogContext()
            //     .WriteTo.Console(new RenderedCompactJsonFormatter())
            //     .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
            //     .WriteTo.File("Log//log.txt", rollingInterval: RollingInterval.Day)
            //     //.WriteTo.Seq("http://localhost:5341/")
            //     .CreateLogger();

            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
           .Enrich.FromLogContext()
           .WriteTo.File("Log//log.txt", rollingInterval: RollingInterval.Day)
           //.WriteTo.Seq("http://localhost:5341")
           .CreateLogger();

            //CreateHostBuilder(args).Build().Run();
            try
            {
                Log.Information("Starting host===========================================================");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>()
                     .UseUrls("http://*:6008");
                 });

    }
}
