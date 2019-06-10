using HRCommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRCommonTools
{
    static internal class PagingParameterInLimiter
    {
        /// <summary>
        /// Chunk the queried pageModel pageSize.
        /// 1- If pagingIn has a too large PageSize, return a new Paging :
        ///     1.1- With the maxPageSize allowed
        ///     1.2- Reprocess page number :    
        ///         1.2.1 - if original pagenumber is 0, the new is 0 too
        ///         1.2.1- else With the pageNumber reprocessed to 0. Unfriendly but OK for a first version.
        /// 2- Else return the PagingIn as is.
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public static PagingParameterInModel LimitPagingIn(PagingParameterInModel pageModel, ushort maxPageSizeAllowed)
        {
            //1-
            if (pageModel != null && pageModel.PageSize > maxPageSizeAllowed)
            {
                //1.1-
                PagingParameterInModel retour = new PagingParameterInModel() { PageSize = maxPageSizeAllowed };
                //1.2.1-
                if (pageModel.PageNumber == 0)
                {
                    retour.PageNumber = 0;
                }
                //1.2.2
                else
                {
                    double ratio = pageModel.PageSize / retour.PageSize;
                    retour.PageNumber = (ushort)(Math.Floor(ratio));

                }
                return retour;
            }
            //2-
            else
            {
                return pageModel;
            }
        }
    }
}
