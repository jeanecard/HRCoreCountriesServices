using HRCommonTools;
using HRCommonTools.Interace;
using HRCoreBordersModel;
using HRCoreBordersRepository;
using HRCoreBordersServices;
using HRCoreCountriesRepository;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//Swagger Dependencies
using NSwag.AspNetCore;
using QuickType;

namespace HRCoreCountriesWebAPI2
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
            //!TODO do correct DI please
            services.AddSingleton<ICoreCountriesService>(new CoreCountriesService(new MongoDBCountriesRepository(Configuration)));
            services.AddSingleton<IHRPaginer<HRBorder>>(new HRPaginer<HRBorder>());
            services.AddSingleton<IHRPaginer<HRCountry>>(new HRPaginer<HRCountry>());

            services.AddSingleton(Configuration);
            services.AddSingleton<ICoreBordersService>(new HRCoreBordersService(new CoreBordersRepository(Configuration)));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // Register the Swagger services
            services.AddSwaggerDocument();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
