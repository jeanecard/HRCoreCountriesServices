using HRCommon;
using HRCommon.Interface;
using HRCommonTools;
using HRCommonTools.Interface;
using HRControllersForker;
using HRControllersForker.Interface;
using HRCoreBordersModel;
using HRCoreBordersRepository;
using HRCoreBordersServices;
using HRCoreCountriesRepository;
using HRCoreCountriesRepository.Interface;
using HRCoreCountriesServices;
using HRCoreRepository.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickType;
using System;

namespace HRCoreCountriesWebAPI2
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private static readonly String _VERSION_FOR_SWAGGER_DISLPAY = "Version 1 Release candidate";
        private static readonly String _NAME_FOR_SWAGGER_DISLPAY = "HR Core Services";

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
            services.AddTransient<IHRCommonForkerUtils, HRCommonForkerUtils>();
            services.AddTransient<IHRBordersControllersForker, HRBordersControllersForker>();
            services.AddTransient<IHRCountriesControllersForker, HRCountriesControllersForker>();
            services.AddTransient<IHRContinentControllerForker, HRContinentControllerForker>();
            services.AddTransient<IHRLangagesByContinentControllerForker, HRLangagesByContinentControllerForker>();
            services.AddTransient<IHRCountriesByContinentControllerForker, HRCountriesByContinentControllerForker>();
            services.AddSingleton(Configuration);

            services.AddTransient<IHRPaginer<HRBorder>, HRPaginer<HRBorder>>();
            services.AddTransient<IHRPaginer<HRCountry>, HRPaginer<HRCountry>>();

            services.AddTransient<IServiceWorkflowOnHRCoreRepository<HRCountry>, HRServiceWorkflowPaginationOnly<HRCountry>>();
            services.AddTransient<IServiceWorkflowOnHRCoreRepository<HRBorder>, HRServiceWorkflowPaginationOnly<HRBorder>>();
            services.AddTransient<ILanguageRepository, MongoDBLanguageRepository>();

            services.AddSingleton<IHRCoreRepository<HRBorder>, PostGISCoreBordersRepository>();
            services.AddSingleton<IHRCoreRepository<HRCountry>, MongoDBCountriesRepository>();

            services.AddTransient<ICoreCountriesService, CoreCountriesService>();
            services.AddTransient<ICoreBordersService, HRCoreBordersService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // Register the Swagger services
            services.AddSwaggerDocument(swagger => {
                swagger.Version = _VERSION_FOR_SWAGGER_DISLPAY;
                swagger.Title = _NAME_FOR_SWAGGER_DISLPAY;
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
