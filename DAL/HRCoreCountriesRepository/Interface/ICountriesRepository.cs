using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesRepository
{
    public interface ICountriesRepository : ISortable, IPaginable
    {
        Task<HRCountry> GetCountryAsync(String id);

        Task<IEnumerable<HRCountry>> GetOrderedCountriesAsync(HRSortingParamModel orderBy);
        Task<IEnumerable<HRCountry>> GetFullCountriesAsync();

        Task<PagingParameterOutModel<HRCountry>> GetOrderedAndPaginatedCountriesAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy);
        Task<PagingParameterOutModel<HRCountry>> GetPaginatedCountriesAsync(PagingParameterInModel pageModel);
    }
}