using HRBordersAndCountriesWebAPI2.Utils;
using HRCommon.Interface;
using HRCommonModels;

namespace XUnitTestControllers.MockAndStubs
{
    internal class HRCommonForkerUtilsStub : IHRCommonForkerUtils
    {
        public bool CanOrderReturn = true;
        public bool CanOrder(HRSortingParamModel orderBy, ISortable service)
        {
            return CanOrderReturn;
        }
    }
}
