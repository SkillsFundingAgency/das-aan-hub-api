
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SFA.DAS.AAN.Domain.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.AAN.Hub.Api.AppStart;


namespace SFA.DAS.AAN.Hub.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment Environment;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();

            config.AddAzureTableStorage(options =>
            {
                options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                options.EnvironmentName = configuration["EnvironmentName"];
                options.PreFixConfigurationKeys = false;
            });
#if DEBUG
            config.AddJsonFile($"appsettings.Development.json", optional: true);
#endif

            Configuration = config.Build();
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SFA.DAS.AAN.Hub.Api", Version = "v1" });
            });
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddSingleton(s => s.GetRequiredService<IOptions<ApplicationSettings>>().Value);

            services.AddDatabaseRegistration(Configuration);
            services.AddControllers();
            services.AddServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SFA.DAS.AAN.Hub.Api v1"));
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHealthChecks("/ping");
            });
        }
    }
}
