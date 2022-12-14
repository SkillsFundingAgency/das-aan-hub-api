using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SFA.DAS.AANHub.Application.Extensions;
using SFA.DAS.AANHub.Data.Extensions;
using SFA.DAS.AANHub.Domain.Configuration;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Configuration.AzureTableStorage;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Api
{

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly string _environmentName;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environmentName = configuration["EnvironmentName"];
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = _environmentName;
                    options.PreFixConfigurationKeys = false;
                });
#if DEBUG
            config.AddJsonFile($"appsettings.Development.json", optional: true);
#endif

            Configuration = config.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (!IsEnvironmentLocalOrDev)
            {
                var azureAdConfiguration = Configuration
                    .GetSection("AzureAd")
                    .Get<AzureActiveDirectoryConfiguration>();

                var policies = new Dictionary<string, string>
                {
                    { "Default", "Default" }
                };

                services.AddAuthentication(azureAdConfiguration, policies);
            }

            services.AddHealthChecks();

            services.AddApplicationInsightsTelemetry();

            services.AddApiVersioning(opt =>
            {
                opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });

            services
                .AddControllers(options =>
                {
                    if (!IsEnvironmentLocalOrDev)
                        options.Conventions.Add(new AuthorizeControllerModelConvention(new List<string>()));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AAN Hub API" });
                c.OperationFilter<SwaggerVersionHeaderFilter>();
            });

            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddSingleton(s => s.GetRequiredService<IOptions<ApplicationSettings>>().Value);

            services.AddAanDataContext(Configuration["ApplicationSettings:DbConnectionString"], _environmentName);
            services.AddApplicationRegistrations();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "SFA.DAS.AANHub.Api v1");
                options.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
            });

            if (!IsEnvironmentLocalOrDev)
            {
                app.UseHealthChecks("/ping", new HealthCheckOptions
                {
                    Predicate = (_) => false,
                    ResponseWriter = (context, report) =>
                    {
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("");
                    }
                });
            }
            app.UseFluentValidationExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private bool IsEnvironmentLocalOrDev =>
            _environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
            || _environmentName.Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
    }
}
