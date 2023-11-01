using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFA.DAS.AANHub.Api.SwaggerExamples;
using SFA.DAS.AANHub.Application.Extensions;
using SFA.DAS.AANHub.Data.Extensions;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Configuration;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Telemetry.Startup;
using Swashbuckle.AspNetCore.Filters;

namespace SFA.DAS.AANHub.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly string _environmentName;

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
            config.AddJsonFile("appsettings.Development.json", true);
#endif

            Configuration = config.Build();
        }

        public IConfiguration Configuration { get; }

        private bool IsEnvironmentLocalOrDev =>
            _environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
            || _environmentName.Equals("DEV", StringComparison.CurrentCultureIgnoreCase);

        public void ConfigureServices(IServiceCollection services)
        {
            if (!IsEnvironmentLocalOrDev)
            {
                var azureAdConfiguration = Configuration
                    .GetSection("AzureAd")
                    .Get<AzureActiveDirectoryConfiguration>();

                var policies = new Dictionary<string, string>
                {
                    {
                        "Default", "Default"
                    }
                };

                services.AddAuthentication(azureAdConfiguration, policies);
            }

            services.AddHealthChecks();

            services
                .AddApplicationInsightsTelemetry()
                .AddTelemetryUriRedaction("firstName,lastName,dateOfBirth");

            services.AddApiVersioning(opt =>
            {
                opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                opt.DefaultApiVersion = new ApiVersion(1, 0);
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
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1",
                        new OpenApiInfo
                        {
                            Title = "AAN Hub API"
                        });

                    options.OperationFilter<SwaggerVersionHeaderFilter>();
                    options.ExampleFilters();
                })
                .AddSwaggerExamplesFromAssemblyOf<PatchMemberExample>();

            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddSingleton(s => s.GetRequiredService<IOptions<ApplicationSettings>>().Value);

            services.AddAanDataContext(Configuration["ApplicationSettings:DbConnectionString"], _environmentName);
            services.AddApplicationRegistrations();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "SFA.DAS.AANHub.Api v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
                });

            if (!IsEnvironmentLocalOrDev)
                app.UseHealthChecks("/ping",
                    new HealthCheckOptions
                    {
                        Predicate = _ => false,
                        ResponseWriter = (context, report) =>
                        {
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("");
                        }
                    });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}