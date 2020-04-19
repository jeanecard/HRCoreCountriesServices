using HRCommonModel;
using HRCommonModels;
using HRControllersForker.Interface;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Http;
using QuickType;
using System;
using System.Threading.Tasks;

namespace HRControllersForker
{
    /// <summary>
    /// 
    /// </summary>
    public class HRCountriesControllersForker : IHRCountriesControllersForker
    {
        private readonly IHRCommonForkerUtils _util = null;
        private HRCountriesControllersForker()
        {
            //Dummy.
        }
        /// <summary>
        /// Constructor for DI.
        /// </summary>
        /// <param name="util"></param>
        public HRCountriesControllersForker(IHRCommonForkerUtils util)
        {
            _util = util;
        }
        /// <summary>
        /// Get a Country by ID (ALPHA2_3CODE)
        /// </summary>
        /// <param name="id">the country ID</param>
        /// <param name="service">the core countries service</param>
        /// <returns>the status code (http) and the Country.</returns>
        public  async Task<(int, HRCountry)> GetFromIDAsync(string id, ICoreCountriesService service)
        {
            //1-
            if (String.IsNullOrEmpty(id))
            {
                //Could not happen as Get(PageModel = null) exists)
                return (StatusCodes.Status400BadRequest, null);
            }
            if (service == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            //2-
            try
            {
                String idToCompare = id.ToUpper();
                Task<HRCountry> countryAction = service.GetCountryAsync(id);
                await countryAction;
                //3-
                if (countryAction.Result != null)
                {

                    //Last check necessary ??
                    HRCountry candidateCountry = countryAction.Result;
                    if ((!String.IsNullOrEmpty(candidateCountry.Alpha2Code) && candidateCountry.Alpha2Code.ToUpper() == idToCompare)
                        || (!String.IsNullOrEmpty(candidateCountry.Alpha3Code) && (candidateCountry.Alpha3Code == idToCompare)))
                    {
                        return (StatusCodes.Status200OK, countryAction.Result);
                    }
                    else
                    {
                        return (StatusCodes.Status404NotFound, null);
                    }
                }
                return (StatusCodes.Status404NotFound, null);
            }
            catch (Exception)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
        }

        /// <summary>
        /// 1- Process PagingInParameter if not supplied
        /// 2- Get the HRCountry from service
        /// 3- Paginate previous result
        /// </summary>
        /// <param name="pageModel">the paging model</param>
        /// <param name="orderBy">The order by clause</param>
        /// <param name="service">the Core countries service</param>
        /// <param name="maxPageSize">the maxPage size allowed for pagination.</param>
        /// <returns></returns>
        public  async Task<(int, PagingParameterOutModel<HRCountry>)> GetFromPagingAsync(
            PagingParameterInModel pageModel,
            HRSortingParamModel orderBy,
            ICoreCountriesService service,
            ushort maxPageSize)
        {
            if (service != null && _util != null)
            {
                if (!_util.CanOrder(orderBy, service))
                {
                    return (StatusCodes.Status400BadRequest, null);
                }

                //!Add tu on this
                if (pageModel.PageSize > maxPageSize)
                {
                    return (StatusCodes.Status413PayloadTooLarge, null);
                }
                try
                {
                    //2-
                    Task<PagingParameterOutModel<HRCountry>> countriesAction = service.GetCountriesAsync(pageModel, orderBy);
                    await countriesAction;
                    //3-
                    return (StatusCodes.Status200OK, countriesAction.Result);

                }
                catch (IndexOutOfRangeException)
                {
                    //!Add tu on this
                    return (StatusCodes.Status416RequestedRangeNotSatisfiable, null);
                }
                catch (Exception)
                {
                    return (StatusCodes.Status500InternalServerError, null);
                }
            }
            else
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
        }

    }
}
