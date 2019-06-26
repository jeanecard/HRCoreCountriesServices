using HRCommonModel;
using HRCommonModels;
using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreBordersServices
{
    public interface ICoreBordersService
    {
        bool IsSortable();
        Task<HRBorder> GetBorderAsync(String borderID);
        Task<PagingParameterOutModel<HRBorder>> GetBordersAsync(PagingParameterInModel pageModel,  HRSortingParamModel orderBy);

        
    }
}
