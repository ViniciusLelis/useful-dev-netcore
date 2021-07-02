namespace UsefulDev.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using UsefulDev.Core;
    using UsefulDev.Core.Services;
    using UsefulDev.Providers.FileGenerators;

    /// <summary>
    /// Api startup
    /// </summary>
    public class Startup
    {
        const string _appTitle = "UsefulDev";
        const string _appDescription = "Tools for developers and QA analysts";

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                             .AllowAnyMethod()
                                             .AllowAnyHeader();
                                  });        
            });

            services.AddControllers();

            SetupApplication(services);

            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.OrderActionsBy((apiDesc) => apiDesc.RelativePath);
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = _appTitle,
                    Description = _appDescription,
                    Version = "v1"
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationProviders providers)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePathBase("/useful-dev");
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", _appTitle);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            providers.Services = app.ApplicationServices;
        }

        private static void SetupApplication(IServiceCollection services)
        {
            var applicationProviders = new ApplicationProviders();

            services.AddHandlers();
            services.AddFileGenerators(applicationProviders);
            services.AddSingleton(applicationProviders);
        }

    }
}
