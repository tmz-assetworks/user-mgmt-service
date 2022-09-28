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
          //  var configuration1 = new ConfigurationBuilder()
          //.AddJsonFile("appsettings.json")
          //.AddJsonFile($"appsettings.{environment.ToString()}.json")
          //.AddEnvironmentVariables()
          //.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //this.ConfigureOAuth(services);
            // services.AddMicrosoftWebApiAuthentication(Configuration);
            //services.AddAuthentication(AzureADDefaults.JwtBearerAuthenticationScheme)
            //.AddAzureADBearer(options => Configuration.Bind("AzureAdN", options));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
            var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";
            services.AddDbContext<UsersService.Infrastructure.DBContext.DBContextCore>(

            //m => m.UseSqlServer(Configuration.GetConnectionString("UserDB")), ServiceLifetime.Transient);
            m => m.UseSqlServer(connectionString), ServiceLifetime.Transient);

            //var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            //var OcppdbName = Environment.GetEnvironmentVariable("OCPP_DB_NAME");
            //var dbName = Environment.GetEnvironmentVariable("Asset_DB_NAME");
            //var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
            //var connectionString = Configuration.GetConnectionString("UserDB");
            //services.AddDbContext<DBContextCore>(
            //  o => o.UseSqlServer(connectionString), ServiceLifetime.Transient);

            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddMediatR(typeof(GetByIdCustomersHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateUserHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateCustomerHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(UpdateUserHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(DeleteUserHandler).GetTypeInfo().Assembly);

            services.AddScoped(typeof(Core.Repositories.Base.IRepository<>), typeof(Repository<>));
            services.AddTransient<IUserRepository, UsersRepository>();
            services.AddMediatR(typeof(CustomerDDLHandler).GetTypeInfo().Assembly);
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
