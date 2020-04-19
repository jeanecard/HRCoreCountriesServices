using HRCommon.Interface;
using HRCommonModels;
using HRControllersForker.Interface;

namespace TemporaryStubsToMoveInXUnitStubs
{
    public class HRCommonForkerUtilsStub : IHRCommonForkerUtils
    {
        public bool CanOrderReturn = true;
        public bool CanOrder(HRSortingParamModel orderBy, ISortable service)
        {
            return CanOrderReturn;
        }
    }
}
