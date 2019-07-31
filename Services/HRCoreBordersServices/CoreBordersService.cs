using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCoreBordersModel;
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
        private readonly static ushort _maxPageSize = 50;
        private readonly ILogger<HRCoreBordersService> _logger = null;
        public static readonly String ALL_CONTINENT_ID = "All";
        public HRCoreBordersService(IHRCoreRepository<HRBorder> repo,
            IServiceWorkflowOnHRCoreRepository<HRBorder> workflow,
            ILogger<HRCoreBordersService> logger)
        {
            _bordersRepository = repo;
            _workflow = workflow;
            if (_workflow != null)
            {
                _workflow.MaxPageSize = _maxPageSize;
            }
            _logger = logger;
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
        /// Get all enum of Region plus the "All" continent and but the Empty one.
        /// 1- Add all Enum but Empty
        /// 2- Add ALL_CONTINENT_ID
        /// 3- Return List.
        /// TODO : async overkill here
        /// </summary>
        /// <returns>All continent.</returns>
        public async Task<IEnumerable<string>> GetContinentsAsync()
        {
            List<String> retour = new List<string>();
            //1-
            await Task.Run(() =>
            {
                Array regionValues = Enum.GetValues(typeof(Region));
                String nameOfi = String.Empty;

                foreach (Region iterator in regionValues)
                {
                    if (iterator != Region.Empty)
                    {
                        retour.Add(iterator.ToString());
                    }
                }
                //2-
                retour.Add(ALL_CONTINENT_ID);
            }
            );
            //3-
            return retour;
        }
        /// <summary>
        /// Get a Continent by name (case sensitive). Empty can not be retrieve.
        /// 1- Return the simplest case : Check id == ALL_CONTINENT_ID
        /// 2- Check if id corresponds to an Region Enum
        /// 3- Return corresponding or String.empty
        /// TODO : async overkill here
        /// </summary>
        /// <param name="id">The continent name (case sensitive)</param>
        /// <returns></returns>
        public async Task<string> GetContinentByIDAsync(String id)
        {
            String retour = String.Empty;
            //1-
            if(id == ALL_CONTINENT_ID)
            {
                return id;
            }
            await Task.Run(() =>
            {
                //2-
                if (!String.IsNullOrEmpty(id)
                    && (Enum.TryParse(id, out Region regionIDEnumValue) && regionIDEnumValue != Region.Empty))
                {
                    retour = id;
                }
            });
            //3-
            return retour;
        }
    }
}
