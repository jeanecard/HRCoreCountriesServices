using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCoreBordersModel;
using HRCoreCountriesServices;
using HRCoreRepository.Interface;
using Microsoft.Extensions.Logging;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreBordersServices
{
    public class HRCoreBordersService : ICoreBordersService
    {
        private readonly IServiceWorkflowOnHRCoreRepository<HRBorder> _workflow = null;
        private readonly IHRCoreRepository<HRBorder> _bordersRepository = null;
        private readonly ICoreCountriesService _hrCountriesService = null;
        private readonly static ushort _maxPageSize = 50;
        private readonly ILogger<HRCoreBordersService> _logger = null;
        public static readonly String ALL_CONTINENT_ID = "All";
        public HRCoreBordersService(IHRCoreRepository<HRBorder> repo,
            IServiceWorkflowOnHRCoreRepository<HRBorder> workflow,
            ILogger<HRCoreBordersService> logger,
            ICoreCountriesService hrCountriesService)
        {
            _bordersRepository = repo;
            _workflow = workflow;
            if (_workflow != null)
            {
                _workflow.MaxPageSize = _maxPageSize;
            }
            _logger = logger;
            _hrCountriesService = hrCountriesService;
        }

        public bool IsSortable()
        {
            if (_bordersRepository != null)
            {
                return _bordersRepository.IsSortable();
            }
            else
            {
                if (_logger != null)
                {
                    _logger.LogError("_bordersRepository is null in HRCoreBordersServices");
                }
                throw new MemberAccessException();
            }
        }

        /// <summary>
        /// Retrieve a specific Border.
        /// 1- Check consistancy
        ///     1.2- Create Task on GetBorder's repository action
        ///     1.3- Return Result action with no more processing
        /// 2- Throw Exception if consistancy is KO.
        /// </summary>
        /// <param name="borderID">a BorderID to retrieve.</param>
        /// <returns>The border with ID. 
        /// Throw MemberAccessException if repository is null.</returns>
        public async Task<HRBorder> GetBorderAsync(String borderID)
        {
            HRBorder retour = null;
            //1-
            if (_bordersRepository != null)
            {
                //1.2-
                using (Task<HRBorder> bordersTask = _bordersRepository.GetAsync(borderID))
                {
                    await bordersTask;
                    //1.3-
                    retour = bordersTask.Result;
                }
            }
            //2-
            else
            {
                if (_logger != null)
                {
                    _logger.LogError("_bordersRepository is null in HRCoreBordersServices");
                }
                throw new MemberAccessException();
            }
            return retour;
        }
        /// <summary>
        /// Get Paginated items.
        /// </summary>
        /// <param name="pageModel">the PagingParameterInModel. Must not be null.</param>
        /// <param name="orderBy">an optionnal orderByClause.</param>
        /// <returns>The corresponding borders paginated an optionnaly</returns>
        public async Task<PagingParameterOutModel<HRBorder>> GetBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy = null)
        {
            PagingParameterOutModel<HRBorder> retour = null;
            if (_workflow == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("_workflow is null in HRCoreBordersServices");
                }
                throw new MemberAccessException();
            }
            if (pageModel == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("pageModel is null in HRCoreBordersServices : GetBordersAsync");
                }
                throw new ArgumentNullException();
            }
            using (Task<PagingParameterOutModel<HRBorder>> retourTask = _workflow.GetQueryResultsAsync(pageModel, orderBy))
            {
                await retourTask;
                retour = retourTask.Result;
            }
            return retour;
        }
        /// <summary>
        /// All Pagination are available
        /// </summary>
        /// <returns></returns>
        public bool IsPaginable()
        {
            return true;
        }
        /// <summary>
        /// Version 1 : Get HRCountry then HRBorder one by one.
        /// 1- Get Corresponding HRCountries to build list of ISO2 codes.
        /// 2- Get HRBorders for list of ISO2
        /// </summary>
        /// <param name="continentId"></param>
        /// <param name="langageId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRBorder>> GetHRBorderByContinentByLanguageAsync(Region region, string langageId)
        {
            if (_hrCountriesService == null || _bordersRepository == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("_hrCountriesService or _bordersRepository is null in HRCoreBordersServices");
                }
                throw new MemberAccessException();
            }
            //1-
            using (Task<IEnumerable<HRCountry>> countriesTask = _hrCountriesService.GetHRCountriesByContinentByLanguageAsync(region, langageId))
            {
                await countriesTask;
                if (countriesTask.IsCompleted)
                {
                    List<HRBorder> retour = new List<HRBorder>();
                    List<string> ids = new List<string>();
                    //2-
                    foreach (HRCountry iter in countriesTask.Result)
                    {
                        ids.Add(iter.Alpha2Code);
                    }
                    using (Task<IEnumerable<HRBorder>> bordersTask = _bordersRepository.GetAsync(ids))
                    {
                        await bordersTask;
                        if (bordersTask.IsCompleted)
                        {
                            return bordersTask.Result;
                        }
                        else
                        {
                            throw new Exception("GetAsync (HRBorders) fail.");
                        }
                    }
                }
                else
                {
                    throw new Exception("GetHRBorderByContinentByLanguageAsync fail");
                }
            }
        }
        /// <summary>
        /// Version 1 : Get HRCountry then HRBorder one by one.
        /// 1- Get Corresponding HRCountries
        /// 2- Foreach HRCountry get single HRBorders and push it in retrun Enumerable
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRBorder>> GetHRBordersByContinentAsync(Region region)
        {
            if (_hrCountriesService == null || _bordersRepository == null)
            {
                if (_logger != null)
                {
                    _logger.LogError("_hrCountriesService or _bordersRepository is null in HRCoreBordersServices");
                }
                throw new MemberAccessException();
            }
            //1-
            using (Task<IEnumerable<HRCountry>> countriesTask = _hrCountriesService.GetHRCountriesByContinentAsync(region))
            {
                await countriesTask;
                if (countriesTask.IsCompleted)
                {
                    List<HRBorder> retour = new List<HRBorder>();
                    List<string> ids = new List<string>();
                    //2-
                    foreach (HRCountry iter in countriesTask.Result)
                    {
                        ids.Add(iter.Alpha2Code);
                    }
                    using (Task<IEnumerable<HRBorder>> bordersTask = _bordersRepository.GetAsync(ids))
                    {
                        await bordersTask;
                        if (bordersTask.IsCompleted)
                        {
                            return bordersTask.Result;
                        }
                        else
                        {
                            throw new Exception("GetAsync (HRBorders) fail.");
                        }
                    }
                }
                else
                {
                    throw new Exception("GetHRCountriesByContinentAsync fail.");
                }
            }

        }
    }
}
