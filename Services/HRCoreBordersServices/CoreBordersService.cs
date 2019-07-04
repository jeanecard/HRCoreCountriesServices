using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCoreBordersModel;
using HRCoreRepository.Interface;
using System;
using System.Threading.Tasks;

namespace HRCoreBordersServices
{
    public class HRCoreBordersService : ICoreBordersService
    {
        private readonly IServiceWorkflowOnHRCoreRepository<HRBorder> _workflow = null;
        private readonly IHRCoreRepository<HRBorder> _bordersRepository = null;
        private readonly static ushort _maxPageSize = 50;
        public HRCoreBordersService(IHRCoreRepository<HRBorder> repo,
            IServiceWorkflowOnHRCoreRepository<HRBorder> workflow)
        {
            _bordersRepository = repo;
            _workflow = workflow;
            if (_workflow != null)
            {
                _workflow.MaxPageSize = _maxPageSize;
            }
        }

        public bool IsSortable()
        {
            if (_bordersRepository != null)
            {
                return _bordersRepository.IsSortable();
            }
            else
            {
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
            //1-
            if (_bordersRepository != null)
            {
                //1.2-
                Task<HRBorder> bordersTask = _bordersRepository.GetAsync(borderID);
                await bordersTask;
                //1.3-
                return bordersTask.Result;
            }
            //2-
            else
            {
                throw new MemberAccessException();
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<PagingParameterOutModel<HRBorder>> GetBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy = null)
        {
            if (_workflow == null)
            {
                throw new MemberAccessException();
            }
            Task<PagingParameterOutModel<HRBorder>> retourTask = _workflow.GetQueryResultsAsync(pageModel, orderBy);
            await retourTask;
            return retourTask.Result;
        }
        /// <summary>
        /// All Pagination are available
        /// </summary>
        /// <returns></returns>
        public bool IsPaginable()
        {
            return true;
        }
    }
}
