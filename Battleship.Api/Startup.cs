using Battleship.Api.Validators;
using Battleship.Business.Utilities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace Battleship.API
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
            services.AddControllers();
            services.AddMemoryCache();

            services
            .AddMvc()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opts.JsonSerializerOptions.IgnoreNullValues = true;
            })
            .AddFluentValidation();

            // Validators
            services.AddTransient<BattleshipOptionsValidator>();

            // Utilities
            services.AddScoped<IBattleshipUtility, BattleshipUtility>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
