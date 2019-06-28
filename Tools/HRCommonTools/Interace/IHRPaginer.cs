using HRCommonModel;
using System.Collections.Generic;

namespace HRCommonTools.Interface
{
    public interface IHRPaginer<T>
    {
        PagingParameterOutModel<T> GetPaginationFromFullList(PagingParameterInModel model, IEnumerable<T> items, ushort maxPageSize = ushort.MaxValue);
        bool IsValid(PagingParameterInModel model, IEnumerable<T> item);
    }
}
