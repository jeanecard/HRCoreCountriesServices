using HRCommon.Interface;
using HRCommonModels;
using HRCommonTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBordersAndCountriesWebAPI2.Utils
{
    /// <summary>
    /// Utils for Forkers
    /// </summary>
    public class HRCommonForkerUtils : IHRCommonForkerUtils
    {
        /// <summary>
        /// Method use to know if all objects used have necessary skills to sort.
        /// </summary>
        /// <param name="orderBy">The ORderBy clause</param>
        /// <param name="service">The ISortable servcice</param>
        /// <returns></returns>
        public  bool CanOrder(HRSortingParamModel orderBy, ISortable service)
        {
            bool retour = true;
            if (orderBy != null && orderBy.IsInitialised())
            {
                if (service != null)
                {
                    if ((!service.IsSortable()) || !HRSortingParamModelDeserializer.IsValid(orderBy))
                    {
                        retour = false;
                    }
                }
                else
                {
                    retour = false;
                }
            }
            return retour;
        }
    }
}
