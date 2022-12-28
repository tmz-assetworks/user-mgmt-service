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
            string containerName = Environment.GetEnvironmentVariable("LOG_CONTAINER_NAME");
            string connectionString = Environment.GetEnvironmentVariable("LOG_CONNECTIONSTRING");
            if (connectionString == null)
            {
                connectionString = MyConfig.GetValue<string>("LOG:CONTAINER_NAME");
                containerName = MyConfig.GetValue<string>("LOG:CONNECTIONSTRING");
            }
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
