using HRCoreCountriesRepository.Interface;
using Microsoft.Extensions.Logging;
using QuickType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreCountriesRepository
{
    public class MongoDBCountryByContinentByLanguageRepository : IHRCountryByContinentByLanguageRepository
    {
        private readonly ILogger<MongoDBCountriesRepository> _logger = null;
        private readonly IHRCountryByContinentRepository _repo = null;
        /// <summary>
        /// Constructor for DI
        /// </summary>
        /// <param name="injectedMongoConfig">Config mongoDB connexion (Mandatory)</param>
        /// <param name="logger">Logger (optionnal)</param>
        public MongoDBCountryByContinentByLanguageRepository(
            ILogger<MongoDBCountriesRepository> logger,
            IHRCountryByContinentRepository countryByContinentRepository
            )
        {
            _logger = logger;
            _repo = countryByContinentRepository;
        }
        private  MongoDBCountryByContinentByLanguageRepository()
        {
            // Dummy
        }
        /// <summary>
        /// 1- Check entries
        /// 2- Process as following :
        ///     2.1- Get All Countries for region
        ///     2.2- Foreach Coutries, if language iso6391_Or_Iso6392_Language exists, add it to result
        /// 3- Return result.
        /// </summary>
        /// <param name="region">a region (Mandatory) </param>
        /// <param name="Iso6391_Or_Iso6392_Language">a iso language code (Mandatory)</param>
        /// <returns>a enumerable collection of HRCountry with iso6391_Or_Iso6392_Language language spoken. Throw ArgumentNullException and Exeption from DB.</returns>
        public async Task<IEnumerable<HRCountry>> GetHRCountriesByContinentByLanguageAsync(Region region, string iso6391_Or_Iso6392_Language)
        {
            List<HRCountry> retour = new List<HRCountry>();
            //1-
            if (_repo == null || String.IsNullOrEmpty(iso6391_Or_Iso6392_Language) )
            {
                throw new ArgumentNullException("No CountriesByContinentRepository or valid iso6391_Or_Iso6392_Language supplied.");
            }
            //2-
            else
            {
                //2.1-
                String upperedIso = iso6391_Or_Iso6392_Language.ToUpper();
                using (Task<IEnumerable<HRCountry>> repoTask = _repo.GetHRCountriesByContinentAsync(region))
                {
                    await repoTask;
                    if(repoTask.Result != null)
                    {
                        try
                        {
                            IEnumerator<HRCountry> enumerator = repoTask.Result.GetEnumerator();
                            HRCountry iteratori = null;
                            //2.2-
                            while (enumerator.MoveNext())
                            {
                                iteratori = enumerator.Current;
                                if (IsLanguageExistInCountry(iteratori, upperedIso))
                                {
                                    retour.Add(iteratori);
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            if(_logger != null)
                            {
                                _logger.LogError(ex.Message);
                            }
                            throw;
                        }
                    }
                }
            }
            //3-
            return retour;
        }
        /// <summary>
        /// 1- Check entries.
        /// 2- Iterate through Languages.
        ///     2.1- return true as soon as isocode is retrieved.
        /// 3- Return false as no isocode has been found.
        /// </summary>
        /// <param name="iteratori">the country to look for language.(Mandatory)</param>
        /// <param name="languagei">the language code to look for.(Mandatory)</param>
        /// <returns>true if language code is found in country Language, false otherwise.</returns>
        private bool IsLanguageExistInCountry(HRCountry iteratori, String upperedIsoCode)
        {
            //1-
            if(iteratori == null || iteratori.Languages == null || String.IsNullOrEmpty(upperedIsoCode) )
            {
                return false;
            }
            //2-
            int languagesCount = iteratori.Languages.Length;
            Language languagei = null;
            for (int i = 0; i < languagesCount; i++)
            {
                languagei = iteratori.Languages[i];
                if (languagei != null
                    && (
                    (languagei.Iso6391 != null && upperedIsoCode == languagei.Iso6391.ToUpper())
                    ||
                    (languagei.Iso6392 != null && upperedIsoCode == languagei.Iso6392.ToUpper()))
                    )
                {
                    //2.1-
                    return true;                  
                }
            }
            //3-
            return false;
        }
    }
}
