using HRCommonModel;
using HRCommonTools.Interace;
using System;
using System.Collections.Generic;

namespace HRCommonTools
{
    /// <summary>
    /// Class to manage Pagination from Input PagingParameterInModel: To Output PagingParameterOutModel.
    /// </summary>
    public class HRPaginer<T> : IHRPaginer<T>
    {
        /// <summary>
        /// Create OutPutPagination from InputPagination.
        /// </summary>
        /// <param name="model">The input Model</param>
        /// <param name="items">The full list of items to paginate</param>
        /// <returns>ArgumentNullException if input args are null, Exception if input is Invalid else return the PagingParameterOutModel expected. </returns>
        public PagingParameterOutModel<T> GetPagination(PagingParameterInModel model, IEnumerable<T> items)
        {
            if (model == null || items == null)
            {
                throw new ArgumentNullException();
            }
            else if (!IsValid(model, items))
            {
                throw new Exception("Invalid PagingParameterInModel");
            }
            else
            {
                PagingParameterOutModel<T> retour = new PagingParameterOutModel<T>();
                IEnumerator<T> enumerator = items.GetEnumerator();
                int itemsCount = GetEnumerableCount(enumerator);

                retour.TotalItemsCount = itemsCount;
                retour.PageSize = model.PageSize;
                retour.TotalPages = (itemsCount / model.PageSize) + 1;
                List<T> pageItems = new List<T>();
                int iterator = 0;
                int startIndex = model.PageSize * (model.PageNumber);
                int endIndex = startIndex + model.PageSize;
                enumerator.Reset();
                while (enumerator.MoveNext())
                {
                    if (iterator >= endIndex)
                    {
                        break;
                    }
                    else if (iterator < endIndex && iterator >= startIndex)
                    {
                        pageItems.Add(enumerator.Current);
                    }
                    iterator++;
                }
                retour.PageItems = pageItems;
                retour.CurrentPage = model.PageNumber;
                return retour;
            }
        }
        /// <summary>
        /// Compute the validity of the input parameter. Can throw Exceptions.
        /// </summary>
        /// <param name="model">The input Pagination Parameter</param>
        /// <param name="items">The full list of items to paginate</param>
        /// <returns>Throw ArgumentNullException if any of input parameters is null, Throw InvalidOperationException if pageSize is 0 else true if input args are valid else false.</returns>
        public bool IsValid(PagingParameterInModel model, IEnumerable<T> items)
        {
            if (items == null || model == null)
            {
                throw new ArgumentNullException();
            }
            else if (model.PageSize == 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                IEnumerator<T> enumerator = items.GetEnumerator();
                int itemsCount = GetEnumerableCount(enumerator);
                bool isPageNumberInPaginationCapacity = false;
                if (itemsCount <= model.PageSize)
                {
                    isPageNumberInPaginationCapacity = (model.PageNumber == 0);
                }
                else
                {
                    isPageNumberInPaginationCapacity = model.PageNumber <= Math.Floor(((double)itemsCount / (double)model.PageSize));
                }
                return isPageNumberInPaginationCapacity;
            }
        }
        /// <summary>
        /// Internal shortcut to avoid casting as array or list the IEnumerable arg.
        /// </summary>
        /// <param name="items">The list of elements to get Count</param>
        /// <returns>Exception or the result count</returns>
        private int GetEnumerableCount(IEnumerator<T> enumerator)
        {
            int retour = 0;
            if (enumerator != null)
            {
                while (enumerator.MoveNext())
                {
                    retour++;
                }
            }
            return retour;
        }
    }
}
