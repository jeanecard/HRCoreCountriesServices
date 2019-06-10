using HRCommonModel;
using System.Collections.Generic;

namespace HRCommonTools.Interace
{
    public interface IHRPaginer<T>
    {
        PagingParameterOutModel<T> GetPagination(PagingParameterInModel model, IEnumerable<T> items, ushort maxPageSize = ushort.MaxValue);
        bool IsValid(PagingParameterInModel model, IEnumerable<T> item);
    }
}
