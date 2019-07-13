using HRCommonModel;
using HRCommonModels;
using HRCoreCountriesServices;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUnitTestControllers
{
    internal class CoreCountriesServiceStub : ICoreCountriesService
    {
        private readonly List<HRCountry> _list = new List<HRCountry>();
        public bool ThrowException = false;
        private Exception _exceptionToThrow = null;

        public Exception ExceptionToThrow { get
            {
                if(_exceptionToThrow == null)
                {
                    _exceptionToThrow = new Exception();
                }
                return _exceptionToThrow;
            }
            internal set
            {
                _exceptionToThrow = value;
            }
        }

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

        /// <summary>
        /// Construct a list of HRCountry from ID String list.
        /// </summary>
        /// <param name="countriesID"></param>
        public CoreCountriesServiceStub(List<String> countriesID)
        {
            if (countriesID != null)
            {
                foreach (String iterator in countriesID)
                {
                    HRCountry countryi = new HRCountry() { Alpha2Code = iterator, Alpha3Code = iterator };
                    _list.Add(countryi);
                }
            }
        }
    }
}
