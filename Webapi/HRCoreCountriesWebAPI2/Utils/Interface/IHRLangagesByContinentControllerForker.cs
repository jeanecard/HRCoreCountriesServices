﻿using HRCoreCountriesServices;
using QuickType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBordersAndCountriesWebAPI2.Utils.Interface
{
    /// <summary>
    /// IHRLangagesByContinentControllerForker util interface
    /// </summary>
    public interface IHRLangagesByContinentControllerForker
    {
        /// <summary>
        /// Get Langages by Contient with HTTP status.
        /// </summary>
        /// <returns></returns>
        Task<(int, IEnumerable<Language>)> GetLangagesByContinentAsync(ICoreCountriesService service, String continent);
    }
}
