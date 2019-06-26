using HRCommonModel;
using HRCommonModels;
using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreBordersRepository.Interface
{
    public interface IHRCoreBordersRepository
    {
        Task<HRBorder> GetBorderAsync(String borderID);
        Task<IEnumerable<HRBorder>> GetOrderedBordersAsync(HRSortingParamModel orderBy);
        Task<IEnumerable<HRBorder>> GetFullBordersAsync();

        Task<PagingParameterOutModel<HRBorder>> GetOrderedAndPaginatedBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy);
        Task<PagingParameterOutModel<HRBorder>> GetPaginatedBordersAsync(PagingParameterInModel pageModel);

        bool IsPaginable();
        bool IsSortable();
    }
}
