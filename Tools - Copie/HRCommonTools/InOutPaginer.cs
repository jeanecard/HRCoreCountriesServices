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
            throw new NotImplementedException();
        }
    }
}
