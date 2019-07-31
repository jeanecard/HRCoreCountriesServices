using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreBordersServices
{
    public interface ICoreBordersService : ISortable, IPaginable
    {
        Task<HRBorder> GetBorderAsync(String borderID);
        Task<PagingParameterOutModel<HRBorder>> GetBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy);
        Task<IEnumerable<string>> GetContinentsAsync();
        Task<string> GetContinentByIDAsync(String id);
    }
}
