using HRCoreCountriesRepository;
using HRCoreCountriesServices;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUnitTestServices.MocksAndStubs
{
    class HRCoreCountriesRepositoryStub : ICountriesRepository
    {
        private List<HRCountry> _countries = new List<HRCountry>();
        private HRCountry _selected = new HRCountry();
        public HRCoreCountriesRepositoryStub(List<String> countries, String countrySelectable)
        {
            if(countries != null)
            {
                foreach(String iter in countries)
                {
                    _countries.Add(new HRCountry() { _id = new MongoDB.Bson.ObjectId(iter) });
                }
            }
            if(!String.IsNullOrEmpty(countrySelectable))
            {
                _selected._id = new MongoDB.Bson.ObjectId(countrySelectable);
            }
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
                List<HRCountry> retour = new List<HRCountry>();
                retour.Add(_selected);
                return retour;
            }
            else
            {
                return _countries;
            }
        }
    }
}


