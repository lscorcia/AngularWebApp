using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AngularWebApp.Infrastructure.Configuration;
using AngularWebApp.Infrastructure.DI;
using AngularWebApp.Infrastructure.Web.Authentication;
using AngularWebApp.Infrastructure.Web.Authentication.Middleware;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using AngularWebApp.Infrastructure.Web.ErrorHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Scrutor;
using Swashbuckle.AspNetCore.Swagger;

namespace AngularWebApp.Web
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
            // Configuration
            var configSourceConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var configArea = configSourceConfig.GetValue<string>("Configuration:Area");
            var configDbConnString = configSourceConfig.GetConnectionString("ConfigDbContext");

            services.Add(new ServiceDescriptor(typeof(IConfiguration),
                provider => new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddEntityFrameworkConfig(configArea, options =>
                        options.UseSqlServer(configDbConnString)
                    )
                    .AddJsonFile("appsettings.json", false)
                    .Build(),
                ServiceLifetime.Singleton));

            // Services registration
            services.Scan(scan => scan
                .FromApplicationDependencies() // 1. Find the concrete classes
                .AddClasses(classes => classes.AssignableTo<IApplicationService>())        //    to register
                .UsingRegistrationStrategy(RegistrationStrategy.Skip) // 2. Define how to handle duplicates
                .AsSelf()    // 2. Specify which services they are registered as
                .WithTransientLifetime()); // 3. Set the lifetime for the services

            // Set up data directory for the Authentication subsystem
            services.AddCustomIdentity<ApplicationUser, ApplicationRole>(options => {
                    options.Stores.MaxLengthForKeys = 128;

                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+\\";
                    options.User.RequireUniqueEmail = true;
                })
                .AddAuthenticationStore(Configuration.GetConnectionString("AuthDbContext"))
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = Configuration.GetValue<string>("JwtToken:Issuer"),
                        ValidAudience = Configuration.GetValue<string>("JwtToken:Audience"),
                        IssuerSigningKey = GetTokenSigningKey(),
                        ClockSkew = TimeSpan.Zero   //the default for this setting is 5 minutes
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddMvc()
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Initialize logging system
            loggerFactory.AddLog4Net();
            
            //
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // Allow SSL Offloading by load balancers
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                    ForwardLimit = null,
                    RequireHeaderSymmetry = false
                });
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // Enable the SSO login using Windows Authentication
            app.UseWhen(
                context => context.Request.Path.StartsWithSegments("/sso"),
                builder => builder.UseMiddleware<WindowsAuthMiddleware>());
            app.UseMiddleware<ReplaceHttp401StatusCodeMiddleware>();

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // Enable extended error information for ASP.net MVC API
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            //
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action?}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        #region Private Helpers
        private SymmetricSecurityKey GetTokenSigningKey()
        {
            var tokenSigningKeyString = Configuration.GetValue<string>("JwtToken:SigningKey");
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSigningKeyString));
        }
        #endregion
    }
}
