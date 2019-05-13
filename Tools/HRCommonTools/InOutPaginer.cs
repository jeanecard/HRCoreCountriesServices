using HRCommonTools.Interace;
using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRCommonTools
{
    /// <summary>
    /// !TODO
    /// </summary>
    public class InOutPaginer<T> : IInOutPaginer<T>
    {
        //!TODO
        public PagingParameterOutModel<T> GetPagination(PagingParameterInModel model, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }
        public bool IsValid(PagingParameterInModel model, IEnumerable<T> items)
        {
            if(items == null || model == null)
            {
                throw new ArgumentNullException();
            }
            else if(model.PageSize == 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                int itemsCount = 0;
                IEnumerator<T> enumerator = items.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    itemsCount++;
                }
                if (model.PageNumber > 0)
                {
                    return model.PageSize * model.PageNumber <= itemsCount;
                }
                else
                {
                    return itemsCount <= model.PageSize;
                }
            }
        }
    }
}
