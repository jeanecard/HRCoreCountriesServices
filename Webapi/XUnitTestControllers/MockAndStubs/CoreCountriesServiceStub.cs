﻿using HRCoreBordersModel;
using HRCoreBordersServices;
using HRCoreCountriesServices;
using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestControllers
{
    class CoreCountriesServiceStub : ICoreCountriesService
    {
        private List<HRCountry> _list = new List<HRCountry>();
        public bool ThrowException = false;

        /// <summary>
        /// Return list or raise exception.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRCountry>> GetCountriesAsync(string id = null)
        {
            await Task.Delay(1);
            if(!String.IsNullOrEmpty(id))
            {
                List<HRCountry> retour = new List<HRCountry>();
                foreach (HRCountry iterator in _list)
                {
                    if(iterator._id.Equals(new MongoDB.Bson.ObjectId(id)))
                    {
                        retour.Add(iterator);
                    }
                }
            }
            if (ThrowException)
            {
                throw new Exception("");
            }
            return _list;
        }
        /// <summary>
        /// Construct a list of HRCountry from ID String list.
        /// </summary>
        /// <param name="countriesID"></param>
        public CoreCountriesServiceStub(List<MongoDB.Bson.ObjectId> countriesID)
        {
            if (countriesID != null)
            {
                foreach (MongoDB.Bson.ObjectId iterator in countriesID)
                {
                    HRCountry countryi = new HRCountry();
                    countryi._id = iterator;
                    _list.Add(countryi);
                }
            }
        }
    }
}
