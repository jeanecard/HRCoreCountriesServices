using HRCommonModel;
using System.Collections.Generic;

namespace HRCommonTools.Interace
{
    public interface IHRPaginer<T>
    {
        PagingParameterOutModel<T> GetPagination(PagingParameterInModel model, IEnumerable<T> items);
        bool IsValid(PagingParameterInModel model, IEnumerable<T> item);
    }
}
