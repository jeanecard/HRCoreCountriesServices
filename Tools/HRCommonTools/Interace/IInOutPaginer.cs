using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRCommonTools.Interace
{
    public interface IInOutPaginer<T>
    {
        /// <summary>
        /// !TODO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        PagingParameterOutModel<T> GetPagination(PagingParameterInModel model, IEnumerable<T> items);
        bool IsValid(PagingParameterInModel model, IEnumerable<T> item);
    }
}
