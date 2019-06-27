using HRCommonModel;
using HRCommonModels;
using HRCommonTools.Interace;
using HRCoreCountriesRepository;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesServices
{
    public class CoreCountriesService : ICoreCountriesService
    {
        private readonly ICountriesRepository _repository = null;
        private readonly IHRPaginer<HRCountry> _paginer = null;
        private readonly static ushort _maxPageSize = 50;
        //Hide default constructor as we must do DI with ICountriesRepository
        private CoreCountriesService()
        {
        }
        //1- Constructor injection of CountiresRepository
        public CoreCountriesService(ICountriesRepository repo, IHRPaginer<HRCountry> paginer)
        {
            //1-
            _repository = repo;
            _paginer = paginer;
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
            //1-
            if (_repository != null)
            {
                //1.1-
                Task<HRCountry> countryTask = _repository.GetCountryAsync(id);
                //1.2-
                HRCountry retour = await countryTask;
                //1.3-
                return retour;
            }
            else
            {
                //2-
                throw new MemberAccessException("CoreCountriesService initialization failed..");
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
                public async Task<PagingParameterOutModel<HRCountry>> GetCountriesAsync(
            PagingParameterInModel pageModel,
            HRSortingParamModel orderBy)
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
            //!TODO
            if (pageModel.PageSize > _maxPageSize)
            {
                throw new IndexOutOfRangeException("Pagination is bounded to maximum : " + _maxPageSize.ToString() + " items.");
            }
            //2-
            Task<IEnumerable<HRCountry>> taskForInternalPagination = null;
            //2.1-
            if (orderBy != null && orderBy.IsInitialised())
            {
                //2.1.1-
                if (_repository.IsSortable())
                {
                    //2.1.1.1-
                    if (_repository.IsPaginable())
                    {
                        Task<PagingParameterOutModel<HRCountry>> countriesTask = null;
                        countriesTask = _repository.GetOrderedAndPaginatedCountriesAsync(pageModel, orderBy);
                        await countriesTask;
                        return countriesTask.Result;
                    }
                    //2.1.1.2-
                    else
                    {
                        taskForInternalPagination = _repository.GetOrderedCountriesAsync(orderBy);
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
                    Task<PagingParameterOutModel<HRCountry>> countriesTask = null;
                    countriesTask = _repository.GetPaginatedCountriesAsync(pageModel);
                    await countriesTask;
                    return countriesTask.Result;
                }
                //2.2.2-
                else
                {
                    taskForInternalPagination = _repository.GetFullCountriesAsync();
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
    }
}
