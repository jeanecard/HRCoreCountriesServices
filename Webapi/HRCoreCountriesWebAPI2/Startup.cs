using HRBordersAndCountriesWebAPI2.Utils;
using HRCommon;
using HRCommonTools;
using HRCommonTools.Interface;
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
using QuickType;

namespace HRCoreCountriesWebAPI2
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Rework DI please
            services.AddSingleton<IHRBordersControllersForker>(new HRBordersControllersForker(new HRCommonForkerUtils()));
            services.AddSingleton<IHRCountriesControllersForker>(new HRCountriesControllersForker(new HRCommonForkerUtils()));
            services.AddSingleton<IHRCommonForkerUtils>(new HRCommonForkerUtils());

            services.AddSingleton<ICoreCountriesService>(new CoreCountriesService((
                new MongoDBCountriesRepository(Configuration)),
                new HRServiceWorkflowPaginationOnly<HRCountry>(new MongoDBCountriesRepository(Configuration), new HRPaginer<HRCountry>())
                ));
            services.AddSingleton<IHRPaginer<HRBorder>>(new HRPaginer<HRBorder>());
            services.AddSingleton<IHRPaginer<HRCountry>>(new HRPaginer<HRCountry>());

            services.AddSingleton(Configuration);
            services.AddSingleton<ICoreBordersService>(new HRCoreBordersService(
                new PostGISCoreBordersRepository(Configuration, new HRPaginer<HRBorder>()),
                new HRServiceWorkflowPaginationOnly<HRBorder>(
                    new PostGISCoreBordersRepository(Configuration,
                        new HRPaginer<HRBorder>()),
                    new HRPaginer<HRBorder>())));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // Register the Swagger services
            services.AddSwaggerDocument(swagger => {
                swagger.Version = "Version 1 Release candidate";
                swagger.Title = "HR Core Services";
                swagger.GenerateEnumMappingDescription = true;
            }) ;
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
