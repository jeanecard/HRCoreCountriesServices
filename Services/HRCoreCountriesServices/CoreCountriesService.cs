using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCoreCountriesRepository.Interface;
using HRCoreRepository.Interface;
using Microsoft.Extensions.Logging;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesServices
{
    public class CoreCountriesService : ICoreCountriesService
    {
        private readonly ILogger<CoreCountriesService> _logger = null;
        private readonly IHRCoreRepository<HRCountry> _countryRepository = null;
        private readonly ILanguageRepository _langRepository = null;
        private readonly IServiceWorkflowOnHRCoreRepository<HRCountry> _workflow = null;
        private readonly static ushort _maxPageSize = 50;
        //Hide default constructor as we must do DI with ICountriesRepository
        private CoreCountriesService()
        {
        }
        //1- Constructor injection of CountiresRepository
        public CoreCountriesService(
            IHRCoreRepository<HRCountry> countryRepo,
            ILanguageRepository langRepo,
            IServiceWorkflowOnHRCoreRepository<HRCountry> workflow,
            ILogger<CoreCountriesService> logger)
        {
            //1-
            _countryRepository = countryRepo;
            _langRepository = langRepo;
            _workflow = workflow;
            if (_workflow != null)
            {
                _workflow.MaxPageSize = _maxPageSize;
            }
            _logger = logger;
        }

        /// <summary>
        /// 1- If reposiory is available
        //  1.1- Launch asynchronous GetCountries on repository
        //  1.2- Give back thread availability waiting for result
        //  1.3- Get back result when wee get it.
        //  2- Else, return basic Exception in this very first version
        /// </summary>
        /// <param name="id">the ISO2 or ISO23 code</param>
        /// <returns>The corresponding Countries. Can throw MemberAccessException if repository is null.</returns>
        public async Task<HRCountry> GetCountryAsync(String id = null)
        {
            HRCountry retour = null;
            //1-
            if (_countryRepository != null)
            {
                //1.1-
                using (Task<HRCountry> countryTask = _countryRepository.GetAsync(id))
                {
                    //1.2-
                    await countryTask;
                    //1.3-
                    retour = countryTask.Result;
                }
            }
            else
            {
                if (_logger != null)
                {
                    _logger.LogError("_repository is null in CoreCountriesService:GetCountryAsync");
                }
                //2-
                throw new MemberAccessException("CoreCountriesService initialization failed..");
            }
            return retour;
        }

        /// <summary>
        /// Not supported but with Repository in version 1.
        /// </summary>
        /// <returns></returns>
        public bool IsSortable()
        {
            return _countryRepository.IsSortable();
        }
        /// <summary>
        /// Not supported but with the Repository in version 1.
        /// </summary>
        /// <returns></returns>
        public bool IsPaginable()
        {
            return _countryRepository.IsPaginable();
        }
        /// <summary>
        /// Return paginated items
        /// </summary>
        /// <param name="pageModel">pageModel, can not be null</param>
        /// <param name="orderBy">orderBy clause, can be null</param>
        /// <returns>The paginated items.</returns>
        public async Task<PagingParameterOutModel<HRCountry>> GetCountriesAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            PagingParameterOutModel<HRCountry> retour = null;
            if (_workflow == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("_workflow is null in CoreCountriesService:GetCountriesAsync");
                }

                throw new MemberAccessException();
            }
            if (pageModel == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("pageModel is null in CoreCountriesService:GetCountriesAsync");
                }

                throw new ArgumentNullException();
            }
            using (Task<PagingParameterOutModel<HRCountry>> retourTask = _workflow.GetQueryResultsAsync(pageModel, orderBy))
            {
                await retourTask;
                retour = retourTask.Result;
            }
            return retour;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<String> GetContinents()
        {
            List<String> retour = new List<String>();
            var values = Enum.GetValues(typeof(Region));
            foreach (Region iter in values)
            {
                retour.Add(iter.ToString());
            }
            return retour;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public String GetContinentByID(string id)
        {
            String idUppered = id.ToUpper();
            var values = Enum.GetValues(typeof(Region));
            foreach (Region iter in values)
            {
                if (iter.ToString().ToUpper() == idUppered)
                {
                    return iter.ToString();
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Language>> GetHRLangagesByContinentAsync(Region region)
        {
            if (_langRepository == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("_repository is null in CoreCountriesService:GetCountryAsync");
                }
                throw new MemberAccessException("CoreCountriesService initialization failed..");
            }
            else
            {
                using (Task<IEnumerable<Language>> task = _langRepository.GetHRLangagesByContinentAsync(region))
                {
                    await task;
                    return task.Result;
                }
            }
        }

        public Task<IEnumerable<HRCountry>> GetHRCountriesByContinentAsync(Region region)
        {
            throw new NotImplementedException();
        }
    }
}
