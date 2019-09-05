using HRCommonModel;
using HRCommonModels;
using HRCoreRepository.Interface;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUnitTestServices.MocksAndStubs
{
    class HRCoreCountriesRepositoryStub : IHRCoreRepository<HRCountry>
    {
        private readonly List<HRCountry> _countries = new List<HRCountry>();
        private readonly HRCountry _selected = new HRCountry();
        public HRCoreCountriesRepositoryStub(List<String> countries, String countrySelectable)
        {
            if (countries != null)
            {
                foreach (String iter in countries)
                {
                    _countries.Add(new HRCountry() { Alpha2Code = iter });
                }
            }
            if (!String.IsNullOrEmpty(countrySelectable))
            {
                _selected.Alpha2Code = countrySelectable;
            }
        }

        public async Task<HRCountry> GetAsync(string id)
        {
            await Task.Delay(1);
            foreach(HRCountry iter in _countries)
            {
                if(iter.Alpha2Code == id)
                {
                    return iter;
                }
            }
            return null;
        }

        public async Task<HRCountry> GetCountryAsync(string id)
        {
            await Task.Delay(1);
            foreach (HRCountry iter in _countries)
            {
                if (iter.Alpha2Code == id)
                {
                    return iter;
                }
            }
            return null;
        }

        public Task<IEnumerable<HRCountry>> GetFullCountriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HRCountry>> GetFullsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Language>> GetHRLangagesByContinentAsync(Region region)
        {
            throw new NotImplementedException();
        }

        public Task<PagingParameterOutModel<HRCountry>> GetOrderedAndPaginatedCountriesAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<PagingParameterOutModel<HRCountry>> GetOrderedAndPaginatedsAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HRCountry>> GetOrderedCountriesAsync(HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HRCountry>> GetOrderedCountriessAsync(HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HRCountry>> GetOrderedsAsync(HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<PagingParameterOutModel<HRCountry>> GetPaginatedCountriesAsync(PagingParameterInModel pageModel)
        {
            throw new NotImplementedException();
        }

        public async Task<PagingParameterOutModel<HRCountry>> GetPaginatedsAsync(PagingParameterInModel pageModel)
        {
            await Task.Delay(1);
            PagingParameterOutModel<HRCountry> retour = new PagingParameterOutModel<HRCountry>();
            retour.PageItems = _countries;
            retour.PageSize = 50;
            retour.CurrentPage = 0;
            retour.TotalItemsCount = 50;
            return retour;
        }

        public bool IsPaginable()
        {
            return true;
        }

        public bool IsSortable()
        {
            return true;
        }
    }
}


