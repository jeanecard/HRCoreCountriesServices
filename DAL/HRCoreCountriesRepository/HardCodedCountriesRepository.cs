using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using QuickType;
using Microsoft.Extensions.Configuration;
using HRDALExceptionLib;

namespace HRCoreCountriesRepository
{
    public class HardCodeCountriesRepository : ICountriesRepository
    {
        //Default Constructor. 
        public HardCodeCountriesRepository()
        {
        }

        public async Task<IEnumerable<HRCountry>> GetCountriesAsync()
        {
            await Task.Delay(1);
            List<HRCountry> retour = new List<HRCountry>();
            HRCountry country = new HRCountry();
            country.Name = "Saucisse";
            country.NativeName = "Wurst";
            country.NumericCode = "33700";
            country.Population = 100000;
            country.Region = Region.Europe;
            country.RegionalBlocs = new RegionalBloc[1];
            country.RegionalBlocs[0] = new RegionalBloc();
            country.RegionalBlocs[0].Name = Name.EuropeanFreeTradeAssociation;
            country.Subregion = "None";
            country.Timezones = null;
            country.Translations = null;
            country.Alpha2Code = "SA";
            country.Alpha3Code = "SAU";
            country.Area = 4521.25;
            country.Capital = "Frankfurt";
            retour.Insert(0, country);
            return retour;
        }
    }
}