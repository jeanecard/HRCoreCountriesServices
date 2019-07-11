using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRCoreBordersModel;
using HRCoreBordersServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBordersAndCountriesWebAPI2.Utils
{
    /// <summary>
    /// Border Controller
    /// </summary>
    public static class HRBordersControllersForker
    {
        /// <summary>
        /// 1- Check input consistance
        /// 2- Call service async
        /// 3- Process result of action as a single HRBorder.
        /// </summary>
        /// <param name="id">the FIPS value searched</param>
        /// <param name="borderService">the Border service</param>
        /// <returns>StatusCode, HRBorder result</returns>

        public static async Task<(int, HRBorder)> GetFromID(String id, ICoreBordersService borderService)
        {
            //1-
            if (String.IsNullOrEmpty(id))
            {
                //Could not happen as Get(PageModel = null) exists)
                return (StatusCodes.Status400BadRequest, null);
            }
            if (borderService == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            //2-
            try
            {
                Task<HRBorder> bordersAction = borderService.GetBorderAsync(id);
                await bordersAction;
                //3-
                HRBorder resultAction = bordersAction.Result;
                if (resultAction != null)
                {
                    if (!String.IsNullOrEmpty(resultAction.FIPS)
                        && resultAction.FIPS.ToUpper() == id.ToUpper())
                    {
                        return (StatusCodes.Status200OK, resultAction);
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
        /// 2- Get the HRBorders from service
        /// 3- Paginate previous result
        /// !Strange we have to âss from query even for an "internal" method ... to untderstand.
        /// </summary>
        /// <param name="pageModel">The Paging Model</param>
        /// <param name="orderBy">The order by clause. Sample : "ISO2;DESC"</param>
        /// <param name="borderService">The countries service</param>
        /// <param name="maxPageSize">The max PAgeSize of pagination</param>
        /// <returns>(http Status Code, PagingParameterOutModel)</returns>
        public static async Task<(int, PagingParameterOutModel<HRBorder>)> GetFromPaging(
            PagingParameterInModel pageModel,
            HRSortingParamModel orderBy,
            ICoreBordersService borderService,
            ushort maxPageSize)
        {
            if (borderService != null)
            {
                if (orderBy != null && orderBy.IsInitialised())
                {
                    if (!borderService.IsSortable())
                    {
                        return (StatusCodes.Status400BadRequest, null);
                    }
                    else if (!HRSortingParamModelDeserializer.IsValid(orderBy))
                    {
                        return (StatusCodes.Status400BadRequest, null);
                    }
                }
                //1-
                //!TODO Add tu on this
                if (pageModel.PageSize > maxPageSize)
                {
                    return (StatusCodes.Status413PayloadTooLarge, null);
                }
                try
                {
                    //2-
                    Task<PagingParameterOutModel<HRBorder>> bordersAction = borderService.GetBordersAsync(pageModel, orderBy);
                    await bordersAction;
                    //3-
                    return (StatusCodes.Status200OK, bordersAction.Result);

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
