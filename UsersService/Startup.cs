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
                     ValidAudience = Configuration["Jwt:Issuer"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                 };
             });
            //services.AddCors(options =>
            //{ 

            //});
            // services.AddTransient<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, Filters.ApiKeyRequirementHandler>();
            //services.AddAuthorization(authConfig =>
            //{
            //    authConfig.AddPolicy("ApiKeyPolicy",
            //        policyBuilder => policyBuilder
            //            .AddRequirements(new Filters.ApiKeyRequirement(new[] { "my-secret-key" })));
            //});
            //services.Configure<JwtBearerOptions>(AzureADDefaults.JwtBearerAuthenticationScheme, options =>
            //{
            //    options.Authority += "/v2.0";
            //    options.TokenValidationParameters.ValidAudiences = new string[] { options.Audience, $"api://{options.Audience}", "7698cbed-7d9f-43b3-b9cd-a4f09b9b55ed", "https://graph.microsoft.com" };
            //    options.TokenValidationParameters.IssuerValidator = AadIssuerValidator.ForAadInstance(options.Authority).ValidateAadIssuer;
            //    options.Events = new JwtBearerEvents();
            //});

            //services.AddTransient<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, Filters.AuthApplicationRole>();
            //services.AddAuthorization(authConfig =>
            //{
            //    authConfig.AddPolicy("UserRole",
            //        policyBuilder => policyBuilder
            //            .AddRequirements(new Filters.RoleRequirement(new[] { "1" })));
            //});
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //  .AddMicrosoftIdentityWebApi(Configuration);
            //services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            //{
            //   // var configManager = new ConfigurationManager<OpenIdConnectConfiguration>("https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever()); //1. need the 'new OpenIdConnect...'
            //    var configManager = new ConfigurationManager<OpenIdConnectConfiguration>("https://login.microsoftonline.com/744aa8b0-bb99-4982-903f-52328216b4be/discovery/keys?appid=7698cbed-7d9f-43b3-b9cd-a4f09b9b55ed", new OpenIdConnectConfigurationRetriever());
            //    OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;
            //    var existingOnTokenValidatedHandler = options.Events.OnTokenValidated;
            //    options.Events.OnTokenValidated = async context =>
            //    {
            //        await existingOnTokenValidatedHandler(context);
            //        options.TokenValidationParameters.ValidateAudience = false;
            //        options.TokenValidationParameters.ValidateIssuerSigningKey = false;
            //        options.TokenValidationParameters.IssuerSigningKeys = config.SigningKeys;
            //        options.TokenValidationParameters.RequireSignedTokens = false;
            //        options.TokenValidationParameters.ValidIssuers = new[] { "https://sts.windows.net/744aa8b0-bb99-4982-903f-52328216b4be", "https://sts.windows.net/744aa8b0-bb99-4982-903f-52328216b4be/" };
            //        options.TokenValidationParameters.ValidAudiences = new[] { "7698cbed-7d9f-43b3-b9cd-a4f09b9b55ed", "https://graph.microsoft.com" };
            //        options.TokenValidationParameters.ValidateIssuer = false;
            //        options.TokenValidationParameters.ValidateActor = false;
            //        options.TokenValidationParameters.ValidateLifetime = false;


            //        // Your code to add extra configuration that will be executed after the current event implementation.
            //        //options.TokenValidationParameters



            //    };
            //});
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Assets.API", Version = "v1" });
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
            });
        }
    }
}
