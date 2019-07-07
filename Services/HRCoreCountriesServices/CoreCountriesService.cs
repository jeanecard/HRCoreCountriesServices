using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCoreRepository.Interface;
using QuickType;
using System;
using System.Threading.Tasks;

namespace HRCoreCountriesServices
{
    public class CoreCountriesService : ICoreCountriesService
    {
        private readonly IHRCoreRepository<HRCountry> _repository = null;
        private readonly IServiceWorkflowOnHRCoreRepository<HRCountry> _workflow = null;
        private readonly static ushort _maxPageSize = 50;
        //Hide default constructor as we must do DI with ICountriesRepository
        private CoreCountriesService()
        {
        }
        //1- Constructor injection of CountiresRepository
        public CoreCountriesService(IHRCoreRepository<HRCountry> repo,
            IServiceWorkflowOnHRCoreRepository<HRCountry> workflow)
        {
            //1-
            _repository = repo;
            _workflow = workflow;
            if (_workflow != null)
            {
                _workflow.MaxPageSize = _maxPageSize;
            }
        }

        /// <summary>
        /// 1- If reposiory is available
        //  1.1- Launch asynchronous GetCountries on repository
        //  1.2- Give back thread availability waiting for result
        //  1.3- Get back result when wee get it.
        //  2- Else, return basic Exception in this very first version
        /// </summary>
        /// <param name="id">the MondoDB ID (hexadecimal)</param>
        /// <returns>The corresponding Countries. Can throw MemberAccessException if repository is null.</returns>
        public async Task<HRCountry> GetCountryAsync(String id = null)
        {
            HRCountry retour = null;
            //1-
            if (_repository != null)
            {
                //1.1-
                using (Task<HRCountry> countryTask = _repository.GetAsync(id))
                {
                    //1.2-
                    await countryTask;
                    //1.3-
                    retour = countryTask.Result;
                }
            }
            else
            {
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
            return _repository.IsSortable();
        }
        /// <summary>
        /// Not supported but with the Repository in version 1.
        /// </summary>
        /// <returns></returns>
        public bool IsPaginable()
        {
            return _repository.IsPaginable();
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
                throw new MemberAccessException();
            }
            if(pageModel == null)
            {
                throw new ArgumentNullException();
            }
            using (Task<PagingParameterOutModel<HRCountry>> retourTask = _workflow.GetQueryResultsAsync(pageModel, orderBy))
            {
                await retourTask;
                retour = retourTask.Result;
            }
            return retour;
        }
    }
}
