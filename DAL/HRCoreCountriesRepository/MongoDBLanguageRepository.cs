using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCoreCountriesRepository.Interface;
using HRCoreRepository.Interface;
using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreCountriesRepository
{
    public class MongoDBLanguageRepository : ILanguageRepository
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Language>> GetHRLangagesByContinentAsync(Region region)
        {
            List<Language> retourStub = new List<Language>();
            Language langi = new Language();
            langi.Iso6391 = "XX";
            langi.Iso6392 = "xx";
            langi.Name = "HRien";
            langi.NativeName = "HRrrrrr";
            retourStub.Add(langi);
            return retourStub;
        }
    }
}
