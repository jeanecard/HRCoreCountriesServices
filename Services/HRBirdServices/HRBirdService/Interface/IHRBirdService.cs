using HRBirdsModel;
using HRCommonModel;
using HRCommonModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdServices
{
    public interface IHRBirdService
    {
        Task<PagingParameterOutModel<HRBirdMainOutput>> GetMainRecordsAsync(
            HRBirdMainInput query,
            PagingParameterInModel pageModel,
            HRSortingParamModel orderBy);
    }
}
