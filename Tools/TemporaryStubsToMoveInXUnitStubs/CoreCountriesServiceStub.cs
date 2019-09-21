using HRCommonModel;
using HRCommonModels;
using HRCoreCountriesServices;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TemporaryStubsToMoveInXUnitStubs
{

    public class CoreCountriesServiceStub : ICoreCountriesService
    {
        private readonly List<HRCountry> _list = new List<HRCountry>();
        private readonly List<String> _originalList = null;
        public bool ThrowException = false;
        private Exception _exceptionToThrow = null;
        private List<Language> _languages = new List<Language>();

        public Exception ExceptionToThrow { get
            {
                if(_exceptionToThrow == null)
                {
                    _exceptionToThrow = new Exception();
                }
                return _exceptionToThrow;
            }
            set
            {
                _exceptionToThrow = value;
            }
        }

        public List<Language> Languages { get => _languages; set => _languages = value; }

        /// <summary>
        /// Return list or raise exception.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRCountry>> GetCountriesAsync(string id = null)
        {
            if (ThrowException)
            {
                throw ExceptionToThrow;
            }
            List<HRCountry> retour = new List<HRCountry>();
            await Task.Delay(1);
            if (!String.IsNullOrEmpty(id))
            {
                foreach (HRCountry iterator in _list)
                {
                    if (iterator._id.Equals(new MongoDB.Bson.ObjectId(id)))
                    {
                        retour.Add(iterator);
                    }
                }
            }
            if (ThrowException)
            {
                throw new Exception("");
            }
            return retour;
        }

        public async  Task<HRCountry> GetCountryAsync(string id)
        {
            await Task.Delay(1);
            if (ThrowException)
            {
                throw ExceptionToThrow;
            }

            if (!String.IsNullOrEmpty(id))
            {
                foreach (HRCountry iterator in _list)
                {
                    if (iterator.Alpha2Code.Equals(id) 
                        || iterator.Alpha3Code.Equals(id))
                    {
                        return iterator;
                    }
                }
            }
            return null;
        }

        public async Task<PagingParameterOutModel<HRCountry>> GetCountriesAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            if (ThrowException)
            {
                throw ExceptionToThrow;
            }
            PagingParameterOutModel<HRCountry> retour = new PagingParameterOutModel<HRCountry>()
            {
                CurrentPage = 0,
                PageItems = _list,
                TotalItemsCount = 5
             };
            return retour;
        }

        public bool IsSortable()
        {
            return true;
        }

        public bool IsPaginable()
        {
            return true;
        }

        public IEnumerable<String> GetContinents()
        {
            if (ThrowException)
            {
                throw ExceptionToThrow;
            }
            List<String> retour = new List<string>();
            foreach(String iterator in _originalList)
            {
                retour.Add(iterator);
            }
            return retour;
        }

        public String GetContinentByID(string id)
        {
            if (ThrowException)
            {
                throw ExceptionToThrow;
            }
            foreach (String iterator in _originalList)
            {
                if (iterator == id)
                    return id;
            }
            return null;
        }


        public async Task<IEnumerable<Language>> GetHRLangagesByContinentAsync(Region region)
        {
            if (ThrowException)
            {
                throw ExceptionToThrow;
            }
            await Task.Delay(1);
            if (Languages != null)
            {
                return Languages;
            }
            return null;
        }

        public async Task<IEnumerable<HRCountry>> GetHRCountriesByContinentAsync(Region region)
        {
            if (ThrowException)
            {
                throw ExceptionToThrow;
            }
            await Task.Delay(1);
            return _list;
        }

        public async Task<IEnumerable<Language>> GetAllLangagesAsync()
        {
            if (ThrowException)
            {
                throw ExceptionToThrow;
            }
            await Task.Delay(1);
            return _languages;
        }

        public Task<IEnumerable<HRCountry>> GetHRCountriesByContinentByLanguageAsync(Region region, string languageID)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Construct a list of HRCountry from ID String list.
        /// </summary>
        /// <param name="countriesID"></param>
        public CoreCountriesServiceStub(List<String> countriesID)
        {
            _originalList = countriesID;
            if (countriesID != null)
            {
                foreach (String iterator in _originalList)
                {
                    HRCountry countryi = new HRCountry() { Alpha2Code = iterator, Alpha3Code = iterator };
                    _list.Add(countryi);
                }
            }
        }
    }
}
