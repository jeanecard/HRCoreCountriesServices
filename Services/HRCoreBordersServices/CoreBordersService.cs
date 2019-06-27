using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRCommonTools.Interace;
using HRCoreBordersModel;
using HRCoreBordersRepository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HRCoreBordersServices
{
    public class HRCoreBordersService : ICoreBordersService
    {
        private readonly IHRCoreBordersRepository _bordersRepository = null;
        private readonly IHRPaginer<HRBorder> _paginer = null;
        private readonly static ushort _maxPageSize = 50;
        public HRCoreBordersService(IHRCoreBordersRepository service, IHRPaginer<HRBorder> paginer)
        {
            _bordersRepository = service;
            _paginer = paginer;
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
                Task<HRBorder> bordersTask = _bordersRepository.GetBorderAsync(borderID);
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
        /// Method used to paginate results. The query in can be ordered or not.
        /// 1- Check that context is consistant before processing.
        ///     1.1- Internal member
        ///     1.2- Input parameters
        /// 2- Process query on repository
        ///  2.1- if OrderBy is supplied
        ///     2.1.1- Is repository is sortable
        ///         2.1.1.1- If repository IsPaginable : Full Query Repository for Pagination and Order and return result
        ///         2.1.1.2- Else : Process partial query on ordering repository capacity for internal pagination
        ///     2.1.2- Else throw NotSupportedException
        ///  2.2- else
        ///     2.2.1- If repository is Paginable, Full Query and return result.
        ///     2.2.2- else Partial repository Query for internal Pagination
        /// 3- This step means that internal Pagination is needed
        ///     3-1- Check that Pagination is valid
        ///     3.2- Process pagination and return.
        /// </summary>
        /// <param name="pageModel">The Input Pagination, can not be null.</param>
        /// <param name="orderBy">The ordering query. Can be null.</param>
        /// <returns>The expected results.Can throw MemberAccessException, ArgumentNullException, NotSupportedException and InvalidOperationException.</returns>
        public async Task<PagingParameterOutModel<HRBorder>> GetBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy = null)
        {
            //1.1-
            if (_bordersRepository == null || _paginer == null)
            {
                throw new MemberAccessException();
            }
            //1.2-
            if (pageModel == null)
            {
                throw new ArgumentNullException();
            }
            //2-
            Task<IEnumerable<HRBorder>> taskForInternalPagination = null;
            //2.1-
            if (orderBy != null && orderBy.IsInitialised())
            {
                //2.1.1-
                if (_bordersRepository.IsSortable())
                {
                    //2.1.1.1-
                    if (_bordersRepository.IsPaginable())
                    {
                        Task<PagingParameterOutModel<HRBorder>> bordersTask = null;
                        bordersTask = _bordersRepository.GetOrderedAndPaginatedBordersAsync(pageModel, orderBy);
                        await bordersTask;
                        return bordersTask.Result;
                    }
                    //2.1.1.2-
                    else
                    {
                        taskForInternalPagination = _bordersRepository.GetOrderedBordersAsync(orderBy);
                    }
                }
                //2.1.2-
                else
                {
                    throw new NotSupportedException("Linq orderBy is not yet implemented.");
                }
            }
            //2.2-
            else
            {
                //2.2.1-
                if (_bordersRepository.IsPaginable())
                {
                    Task<PagingParameterOutModel<HRBorder>> bordersTask = null;
                    bordersTask = _bordersRepository.GetPaginatedBordersAsync(pageModel);
                    await bordersTask;
                    return bordersTask.Result;
                }
                //2.2.2-
                else
                {
                    taskForInternalPagination = _bordersRepository.GetFullBordersAsync();
                }
            }
            //3-
            //3.1-
            if (!_paginer.IsValid(pageModel, taskForInternalPagination.Result))
            {
                throw new InvalidProgramException();
            }
            //3.2-
            else
            {
                return _paginer.GetPaginationFromFullList(pageModel, taskForInternalPagination.Result, _maxPageSize);
            }
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
