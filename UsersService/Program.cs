using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

using UsersService.Api;

namespace UsersService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            //string connectionString = MyConfig.GetValue<string>("LogsUrl:Url");
            //string containerName = MyConfig.GetValue<string>("LogsUrl:containerName");
            var containerName = Environment.GetEnvironmentVariable("ContainerName");
            string connectionString = Environment.GetEnvironmentVariable("ConnectionStringLogs");
            // string connectionString = "DefaultEndpointsProtocol=https;AccountName=assetswork;AccountKey=Rk7iyAEtGHdMWfojFlyE23dXYsMDUkH1zvLghSjWW9kZX7Ecv6wuJuvRifNQfOChKmY5d1Hvx7mE+AStxFztQw==;EndpointSuffix=core.windows.net";
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console().WriteTo.Debug(outputTemplate: DateTime.Now.ToString()).WriteTo.File("./logs/log-.txt", rollingInterval: RollingInterval.Day)
                 .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
                 .WriteTo.AzureBlobStorage(connectionString, LogEventLevel.Information,
                        containerName, "{yyyy}{MM}{dd}.txt", null, false, TimeSpan.FromMinutes(1), null, true).CreateLogger();
            try
            {
                Log.Information("Starting Portal-Service !");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpected !");
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
