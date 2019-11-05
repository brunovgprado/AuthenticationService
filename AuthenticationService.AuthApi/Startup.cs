using System;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

using AuthenticationService.Application.Service;
using AuthenticationService.Data.Context;
using AuthenticationService.Data.Repository;
using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Interfaces.Services;
using AuthenticationService.Domain.Services;
using AuthenticationService.AuthApi.ConfigurationsApi;
using AuthenticationService.AuthApi.Services;

namespace AuthenticationService.AuthApi
{
    public class Startup
    {
/*         public IHostingEnvironment HostingEnvironment { get; private set; }
        public IConfiguration Configuration { get; private set; } */

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            // this.HostingEnvironment = env;
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<TokenGenerationService>();
            services.AddTransient<UsuarioValidator>();
            services.AddDbContext<AuthenticationContext>();
            //services.AddAutomapper();

            services.AddMvc(config => {
                config.ReturnHttpNotAcceptable = true;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            var keyHasherService = new KeyHasherService(SHA512.Create());
            services.AddSingleton(keyHasherService);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info()
                {
                    Title = "Authentication Api",
                    Version = "v1",
                    Contact = new Contact()
                    {
                        Name = "Bruno",
                        Email = "brunomcp2010@gmail.com",
                        Url = "https://github.com/brunovitorprado"
                    }
                });

                options.AddSecurityDefinition(
                    "Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Autenticação baseada em Json Web Token (JWT)",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
            });

            #region JWT configurations

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {                        

                        ValidateIssuer = true,
                        ValidIssuer = Configuration["TokenConfigurations:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = Configuration["TokenConfigurations:Audience"],

                        ValidateIssuerSigningKey = true,                        

                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication Api v1");
            });

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
