using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Evento.Api.Contracts;
using Evento.Api.Model;
using Evento.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Prometheus;

namespace Evento.Api
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
            services.AddScoped<IIdGenerator, IdGenerator>();
            services.AddScoped<IResourceLocator, FileLocator>();
            services.AddScoped<ISchemaProvider, SchemaProvider>();
            services.AddScoped<IResourceElements, ResourceElements>();
            services.AddScoped<IPayloadValidator, PayloadValidator>();
            services.AddScoped<ICloudEventsHandler, CloudEventsHandler>();
            services.AddScoped<IMessageSenderFactory, MessageSenderFactory>();
            services.AddMultiTenant<EventoTenantInfo>().WithConfigurationStore();

            // Auth0 bearer token configuration
            var domain = $"https://{Configuration["Auth0:Domain"]}/";
            services
                .AddAuthentication(JwtBearerDefaults
                    .AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = domain;
                    options.Audience = Configuration["Auth0:Audience"];
                });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Evento.Api", Version = "v1" });
            });
            services.AddControllers().AddNewtonsoftJson();
            services.AddHealthChecks().ForwardToPrometheus();
            //services.Configure<KestrelServerOptions>(
            //    Configuration.GetSection("Kestrel"));
            //services.AddHttpsRedirection(options => options.HttpsPort = 443);
            IdentityModelEventSource.ShowPII = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evento.Api v1"));

            //app.UseHttpsRedirection();

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Health-check and Prometheus configuration
            app.UseHealthChecks("/health");
            app.UseMetricServer();
            app.UseHttpMetrics();
        }
    }
}
