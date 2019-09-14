using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesServices
{
    public interface ICoreCountriesService : ISortable, IPaginable
    {
        Task<HRCountry> GetCountryAsync(String id);
        Task<PagingParameterOutModel<HRCountry>> GetCountriesAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy);

        IEnumerable<String> GetContinents();
        String GetContinentByID(String id);

        Task<IEnumerable<Language>> GetHRLangagesByContinentAsync(Region region);

        Task<IEnumerable<HRCountry>> GetHRCountriesByContinentAsync(Region region);
    }
}