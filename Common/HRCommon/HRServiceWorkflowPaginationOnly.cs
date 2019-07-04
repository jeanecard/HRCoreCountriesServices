using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCommonTools.Interface;
using HRCoreRepository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCommon
{
    public class HRServiceWorkflowPaginationOnly<T> : IServiceWorkflowOnHRCoreRepository<T>
    {
        private readonly IHRCoreRepository<T> _repository = null;
        private readonly IHRPaginer<T> _paginer = null;
        private ushort _maxPageSize = 50;

        /// <summary>
        /// TODO
        /// </summary>
        ushort IServiceWorkflowOnHRCoreRepository<T>.MaxPageSize
        {
            get
            {
                return _maxPageSize;
            }
            set
            {
                _maxPageSize = value;
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        private HRServiceWorkflowPaginationOnly()
        {
            //Private for DI.
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="paginer"></param>
        public HRServiceWorkflowPaginationOnly(IHRCoreRepository<T> repo, IHRPaginer<T> paginer)
        {
            _repository = repo;
            _paginer = paginer;
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
        public async Task<PagingParameterOutModel<T>> GetQueryResultsAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            //1.1-
            if (_repository == null || _paginer == null)
            {
                throw new MemberAccessException();
            }
            //1.2-
            if (pageModel == null)
            {
                throw new ArgumentNullException();
            }
            //2-
            IEnumerable<T> internalPagination = null;
            //2.1-
            if (orderBy != null && orderBy.IsInitialised())
            {
                //2.1.1-
                if (_repository.IsSortable())
                {
                    //2.1.1.1-
                    if (_repository.IsPaginable())
                    {
                        PagingParameterOutModel<T> retourPaginable = null;

                        using (Task<PagingParameterOutModel<T>> itemsTask = _repository.GetOrderedAndPaginatedsAsync(pageModel, orderBy))
                        {
                            await itemsTask;
                            retourPaginable = itemsTask.Result;
                        }
                        return retourPaginable;
                    }
                    //2.1.1.2-
                    else
                    {
                        using (Task<IEnumerable<T>> taskForInternalPagination = _repository.GetOrderedsAsync(orderBy))
                        {
                            await taskForInternalPagination;
                            internalPagination = taskForInternalPagination.Result;
                        }
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
                if (_repository.IsPaginable())
                {
                    PagingParameterOutModel<T> retourPaginable = null;
                    using (Task<PagingParameterOutModel<T>> bordersTask = _repository.GetPaginatedsAsync(pageModel))
                    {
                        await bordersTask;
                        retourPaginable = bordersTask.Result;
                    }
                    return retourPaginable;
                }
                //2.2.2-
                else
                {
                    using (Task<IEnumerable<T>> taskForInternalPagination = _repository.GetFullsAsync())
                    {
                        await taskForInternalPagination;
                        internalPagination = taskForInternalPagination.Result;
                    }
                }
            }
            //3-
            //3.1-
            if (!_paginer.IsValid(pageModel, internalPagination))
            {
                throw new InvalidProgramException();
            }
            //3.2-
            else
            {
                return _paginer.GetPaginationFromFullList(pageModel, internalPagination, _maxPageSize);
            }
        }
    }
}
