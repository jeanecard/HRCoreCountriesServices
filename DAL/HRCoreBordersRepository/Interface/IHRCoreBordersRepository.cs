using HRCommonModels;
using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreBordersRepository.Interface
{
    public interface IHRCoreBordersRepository
    {
        Task<IEnumerable<HRBorder>> GetBordersAsync(String borderID);
        Task<IEnumerable<HRBorder>> GetBordersAsync(HRSortingParamModel orderBy);
        bool IsSortable();
    }
}
