using System;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Ketchup.Core;
using Ketchup.Core.Configurations;
using Ketchup.Core.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Ketchup.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            AppConfig.Configuration = (IConfigurationRoot)configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appConfig = new Configurations.AppConfig();
            services.AddCors(option =>
            {
                option.AddPolicy("cors", build => { build.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//�Ƿ���֤Issuer
                        ValidateAudience = false,//�Ƿ���֤Audience
                        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                        ClockSkew = TimeSpan.FromSeconds(7200),
                        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                        ValidAudience = appConfig.Gateway.Key,//Audience
                        ValidIssuer = appConfig.Gateway.Key,//Issuer���������ǰ��ǩ��jwt������һ��
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.Gateway.Secret))//�õ�SecurityKey
                    };
                });

            services.AddControllers().AddNewtonsoftJson();
            services.AddGrpc();

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add things to the Autofac ContainerBuilder.
            builder.AddCoreService().RegisterModules();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceLocator.Current = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("cors");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseKetchup();
        }
    }
}
