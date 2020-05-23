using ControllersForkerTools;
using ControllersForkerTools.Utils;
using ControllersForkerTools.Utils.Interface;
using GeonameServices.Interface;
using GeonameSrvices;
using HRBirdServices;
using HRBirdsRepository;
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
using Microsoft.Extensions.Hosting;
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
        private readonly string _ALLOW_SPECIFIC_ORIGIN = "_myAllowSpecificOrigins";

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
            services.AddTransient<IHRCountriesByContinentByLangageControllerForker, HRCountriesByContinentByLangageControllerForker>();
            services.AddTransient<IHRBordersByContinentByLangageControllerForker, HRBordersByContinentByLangageControllerForker>();
            services.AddTransient<IHRBordersByContinentControllerForker, HRBordersByContinentControllerForker>();
            services.AddTransient<IHRPaginer<HRBorder>, HRPaginer<HRBorder>>();
            services.AddTransient<IHRPaginer<HRCountry>, HRPaginer<HRCountry>>();

            services.AddTransient<IServiceWorkflowOnHRCoreRepository<HRCountry>, HRServiceWorkflowPaginationOnly<HRCountry>>();
            services.AddTransient<IServiceWorkflowOnHRCoreRepository<HRBorder>, HRServiceWorkflowPaginationOnly<HRBorder>>();
            services.AddTransient<ILanguageRepository, MongoDBLanguageRepository>();
            services.AddTransient<IHRCountryByContinentRepository, MongoDBCountryByContinentRepository>();

            services.AddSingleton<IHRCoreRepository<HRBorder>, PostGISCoreBordersRepository>();
            services.AddSingleton<IHRCoreRepository<HRCountry>, MongoDBCountriesRepository>();
            services.AddSingleton<IHRCountryByContinentByLanguageRepository, MongoDBCountryByContinentByLanguageRepository>();

            services.AddTransient<ICoreCountriesService, CoreCountriesService>();
            services.AddTransient<ICoreBordersService, HRCoreBordersService>();
            services.AddTransient<IHRGeonameService, HRGeonameService>();

            services.AddTransient<IHRBirdService, HRBirdService>();
            services.AddTransient<IHRBirdControllersForker, HRBirdControllersForker>();
            services.AddTransient<IHRBirdRepository, HRBirdRepository>();





            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            // Register the Swagger services
            services.AddSwaggerDocument(swagger => {
                swagger.Version = _VERSION_FOR_SWAGGER_DISLPAY;
                swagger.Title = _NAME_FOR_SWAGGER_DISLPAY;
                swagger.GenerateEnumMappingDescription = true;
            }) ;

            //Allow CORS
            services.AddCors(options =>
            {
                options.AddPolicy(_ALLOW_SPECIFIC_ORIGIN,
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseCors(_ALLOW_SPECIFIC_ORIGIN);
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
