using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using UsersService.Api.DataModel;
using System.Text;
using UsersService.Api.Service;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using UsersService.Infrastructure.DBContext;
using UsersService.Core.Repositories;
using UsersService.Infrastructure.Repositories.Customer;
using MediatR;
using UsersService.Application.Handlers.Assets.CommandHandlers;
using UsersService.Application.Handlers.Users.QueryHandlers.Customer;
using System.Reflection;
using UsersService.Application.Handlers.Users.CommandHandlers.Customer;
using UsersService.Application.Handlers.Users.CommandHandlers.Users;
using UsersService.Application.Handlers.Users.CommandHandlers;
using UsersService.Infrastructure.Repositories.Repository;
using UsersService.Core.Repositories.Users;
using UsersService.Infrastructure.Repositories.Assets;
using UsersService.Application.Handlers.Users.QueryHandlers.Users;
using UsersService.Infrastructure.Helpers;

namespace UsersService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Dictionary<string, string> myConfiguration = new Dictionary<string, string>
                {
                    {"AzureAd:Instance",Environment.GetEnvironmentVariable("AZUREAD_INSTANCE")},
                    {"AzureAd:Domain",Environment.GetEnvironmentVariable("AZUREAD_DOMAIN")},
                    {"AzureAd:clientId", Environment.GetEnvironmentVariable("AZUREAD_CID")},
                    {"AzureAd:TenantId", Environment.GetEnvironmentVariable("AZUREAD_TID")},
                    {"AzureAd:audience",Environment.GetEnvironmentVariable("AZUREAD_AUD")},
                };
            IConfiguration configurationENV = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();

            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_USER_PASSWORD");
            var dbUserName = Environment.GetEnvironmentVariable("DB_LOGIN_USERNAME");
            var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID={dbUserName};Password={dbPassword}";
            if (dbHost == null)
            {
                connectionString = Configuration.GetConnectionString("AssetsDB");
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd1"));
                Environment.SetEnvironmentVariable("NOTIFICATIONAPI", Configuration["BaseUrl:NOTIFICATIONAPI"].ToString());
                services.AddControllers();
            }
            else
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configurationENV.GetSection("AzureAd"));
                services.AddControllers();
            }
            services.AddDbContext<UsersService.Infrastructure.DBContext.DBContextCore>(m => m.UseSqlServer(connectionString), ServiceLifetime.Transient);
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetByIdCustomersHandler).GetTypeInfo().Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateUserHandler).GetTypeInfo().Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateCustomerHandler).GetTypeInfo().Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UpdateUserHandler).GetTypeInfo().Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DeleteUserHandler).GetTypeInfo().Assembly));

            services.AddScoped(typeof(Core.Repositories.Base.IRepository<>), typeof(Repository<>));
            services.AddTransient<IUserRepository, UsersRepository>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CustomerDDLHandler).GetTypeInfo().Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DeleteUserByIdCommandHandler).GetTypeInfo().Assembly));

            services.AddScoped<TokenBase>();


            services.AddControllers();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserService.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "www.Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });


            });

            
            services.AddAutoMapper(typeof(Startup));
            services.AddHealthChecks()
                .AddCheck<UsersServiceHealthCheck>("example_health_check");
        }

       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assets.API v1"));
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assets.API v1"));
            }
            //db.Database.EnsureCreated();
            //  app.UseHttpsRedirection();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            //  app.UseAuthentication();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseCors(builder =>
                  builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                });
            });
        }
    }
}
