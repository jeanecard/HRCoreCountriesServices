using QuickType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreCountriesRepository.Interface
{
    public interface ILanguageRepository
    {
        Task<IEnumerable<Language>> GetHRLangagesByContinentAsync(Region region);
    }
}
