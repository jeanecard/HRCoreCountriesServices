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
                    _countries.Add(new HRCountry() { _id = new MongoDB.Bson.ObjectId(iter) });
                }
            }
            if (!String.IsNullOrEmpty(countrySelectable))
            {
                _selected._id = new MongoDB.Bson.ObjectId(countrySelectable);
            }
        }

        public Task<HRCountry> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrun selected or full list.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRCountry>> GetCountriesAsync(string id = null)
        {
            await Task.Delay(1);
            if (id != null)
            {
                List<HRCountry> retour = new List<HRCountry>() { _selected };
                return retour;
            }
            else
            {
                return _countries;
            }
        }

        public Task<HRCountry> GetCountryAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HRCountry>> GetFullCountriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HRCountry>> GetFullsAsync()
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

        public Task<PagingParameterOutModel<HRCountry>> GetPaginatedsAsync(PagingParameterInModel pageModel)
        {
            throw new NotImplementedException();
        }

        public bool IsPaginable()
        {
            throw new NotImplementedException();
        }

        public bool IsSortable()
        {
            throw new NotImplementedException();
        }
    }
}


