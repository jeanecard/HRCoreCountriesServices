using HRCoreCountriesRepository.Interface;
using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestServices.MocksAndStubs
{
    /// <summary>
    /// 
    /// </summary>
    public class LanguageRepositoryStub : ILanguageRepository
    {
        private List<Language> _langs = new List<Language>();

        public List<Language> Langs { get => _langs; }

        public async Task<IEnumerable<Language>> GetHRAllLangagesAsync()
        {
            await Task.Delay(1);
            return _langs;
        }

        public async Task<IEnumerable<Language>> GetHRLangagesByContinentAsync(Region region)
        {
            await Task.Delay(1);
            return _langs;
        }
    }
}
