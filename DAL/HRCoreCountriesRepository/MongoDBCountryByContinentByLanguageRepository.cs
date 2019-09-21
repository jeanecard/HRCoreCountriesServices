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
        /// TODO
        /// </summary>
        /// <param name="injectedMongoConfig"></param>
        /// <param name="logger"></param>
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
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="Iso6391_Or_Iso6392_Language"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRCountry>> GetHRCountriesByContinentByLanguageAsync(Region region, string iso6391_Or_Iso6392_Language)
        {
            List<HRCountry> retour = new List<HRCountry>();
            if (_repo == null || String.IsNullOrEmpty(iso6391_Or_Iso6392_Language) )
            {
                throw new ArgumentNullException("No CountriesByContinentRepository supplied.");
            }
            else
            {
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
                            Language languagei = null;
                            while (enumerator.MoveNext())
                            {
                                iteratori = enumerator.Current;
                                if (iteratori != null && iteratori.Languages != null)
                                {
                                    int languagesCount = iteratori.Languages.Length;
                                    for (int i = 0; i < languagesCount; i++)
                                    {
                                        languagei = iteratori.Languages[i];
                                        if (languagei != null
                                            && (
                                            (languagei.Iso6391 != null && upperedIso == languagei.Iso6391.ToUpper())
                                            ||
                                            (languagei.Iso6392 != null && upperedIso == languagei.Iso6392.ToUpper()))
                                            )
                                        {
                                            retour.Add(iteratori);
                                            break;
                                        }
                                    }
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
            return retour;
        }
    }
}
