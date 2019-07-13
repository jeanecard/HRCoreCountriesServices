using HRCommon.Interface;
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
    /// Border Controller.
    /// </summary>
    public class HRBordersControllersForker : IHRBordersControllersForker
    {
        private readonly IHRCommonForkerUtils _util = null;
        /// <summary>
        /// Not Allowed.
        /// </summary>
        private HRBordersControllersForker()
        {

        }
        /// <summary>
        /// Constructor for DI.
        /// </summary>
        public HRBordersControllersForker(IHRCommonForkerUtils util)
        {
            _util = util;
        }

        
        /// <summary>
        /// 1- Check input consistance.
        /// 2- Call service async.
        /// 3- Process result of action as a single HRBorder.
        /// </summary>
        /// <param name="id">the FIPS value searched.</param>
        /// <param name="borderService">the Border service.</param>
        /// <returns>StatusCode, HRBorder result.</returns>

        public async Task<(int, HRBorder)> GetFromIDAsync(String id, ICoreBordersService borderService)
        {
            //1-
            if(_util == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
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
        /// 1- Check consistency before calling service
        ///     1.1- Service must be supplied
        ///     1.2- If OrderBy is supplied, check that service have the skill to order and that the OrderBy is valid.
        ///     1.3- PageModel.PageSize must be lower than MaxPage
        /// 2- Get the HRBorders from service
        ///     2.1- If result is OK, return code200
        ///     2.2- Else if IndexOutOfRangeException is catch return Status416RequestedRangeNotSatisfiable
        ///     2.3 Else for any other Exception return Status500InternalServerError
        /// </summary>
        /// <param name="pageModel">The Paging Model.</param>
        /// <param name="orderBy">The order by clause. Sample : "ISO2;DESC".</param>
        /// <param name="borderService">The countries service.</param>
        /// <param name="maxPageSize">The max PageSize of pagination.</param>
        /// <returns>(http Status Code, PagingParameterOutModel).</returns>
        public async Task<(int, PagingParameterOutModel<HRBorder>)> GetFromPagingAsync(
            PagingParameterInModel pageModel,
            HRSortingParamModel orderBy,
            ICoreBordersService borderService,
            ushort maxPageSize)
        {
            //1-
            //1.1-
            if (borderService == null || _util == null)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            //1.2-
            if (!_util.CanOrder(orderBy, borderService))
            {
                return (StatusCodes.Status400BadRequest, null);
            }
            //1.3-
            if (pageModel.PageSize > maxPageSize)
            {
                return (StatusCodes.Status413PayloadTooLarge, null);
            }
            try
            {
                //2-
                Task<PagingParameterOutModel<HRBorder>> bordersAction = borderService.GetBordersAsync(pageModel, orderBy);
                await bordersAction;
                //2.1-
                return (StatusCodes.Status200OK, bordersAction.Result);

            }
            //2.2-
            catch (IndexOutOfRangeException)
            {
                return (StatusCodes.Status416RequestedRangeNotSatisfiable, null);
            }
            //2.3-
            catch (Exception)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
        }
    }
}

