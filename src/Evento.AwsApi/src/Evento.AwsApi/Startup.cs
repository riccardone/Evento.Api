using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Text.Json.Serialization;
using Evento.Api.Contracts;
using Evento.Api.Model;
using Evento.Api.Services;
using Microsoft.OpenApi.Models;

namespace Evento.AwsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IIdGenerator, IdGenerator>();
            services.AddScoped<ISchemaProvider, SchemaProvider>();
            services.AddScoped<IResourceElements, ResourceElements>();
            services.AddScoped<IPayloadValidator, PayloadValidator>();
            services.AddScoped<ICloudEventsHandler, CloudEventsHandler>();
            services.AddScoped<IResourceLocator, FileLocator>();
            services.AddScoped<IMessageSenderFactory, MessageSenderFactory>();
            services.AddScoped<ISchemaProvider, SchemaProvider>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Evento.Api", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddMultiTenant<EventoTenantInfo>().WithConfigurationStore();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            }).AddNewtonsoftJson();
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "Evento.Api v1");
                c.RoutePrefix = string.Empty;
            });

            app
                .UseHsts()
                .UseRouting()
                .UseMultiTenant()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapControllerRoute("default", "{__tenant__=}/{controller=}/{action=}");
                })
                .UseHealthChecks("/health");
        }
    }
}
