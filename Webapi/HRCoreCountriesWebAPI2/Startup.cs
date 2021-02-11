using AutoMapper;
using ControllersForkerTools;
using ControllersForkerTools.Utils;
using ControllersForkerTools.Utils.Interface;
using HRBirdRepository;
using HRBirdRepository.Interface;
using HRBirdService;
using HRBirdService.Interface;
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
using HREFBirdRepository.Models;
using HRGeoLocator.Services;
using HRGeoLocator.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
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
            services.AddTransient<IWebCamService, WebCamService>();
            services.AddTransient<IHRBirdService, HRBirdServices.HRBirdService>();
            services.AddTransient<IHRBirdControllersForker, HRBirdControllersForker>();
            services.AddTransient<IHRBirdRepository, HRBirdsRepository.HRBirdRepository>();
            services.AddTransient<IHRBirdSubmissionRepository, HRBirdSubmissionRepository>();
            services.AddTransient<IBirdsSubmissionService, BirdsSubmissionService>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(Configuration, "AzureAd");




            // Auto Mapper Configurations


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new AutoMapperProfile());
            //});

            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);

            services.AddDbContext<gxxawt_obddnfContext>(options =>
            options.UseNpgsql("host = db.qgiscloud.com; Username = ; Password = ; Database = "));// TODO !!
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            // Register the Swagger services
            services.AddSwaggerDocument(swagger => {
                swagger.Version = _VERSION_FOR_SWAGGER_DISLPAY;
                swagger.Title = _NAME_FOR_SWAGGER_DISLPAY;
                swagger.GenerateEnumMappingDescription = true;
            }) ;
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            //Allow CORS
            services.AddCors(options =>
            {
                options.AddPolicy(_ALLOW_SPECIFIC_ORIGIN,
                builder =>
                {
                    builder.WithOrigins("https://hrnithoonfirebase.firebaseapp.com",
                                        "https://hrcorebordersservicesv-3-1.azurewebsites.net/",
                                        "http://localhost:4200");
                });
            });
            services.AddControllers();
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

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
