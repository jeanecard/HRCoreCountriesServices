using HRCommonModels;
using HRControllersForker;
using System;
using TemporaryStubsToMoveInXUnitStubs;
using Xunit;

namespace XUnitTestControllers
{
    /// <summary>
    /// Test class for HRCommonForkerUtils
    /// </summary>
    public class HRCommonForkerUtilsTest
    {
        /// <summary>
        /// Test que CanOrder retourne faux si :
        /// OrderBy est non null
        /// Service est non null
        /// OrderBy est invalid
        /// </summary>
        [Fact]
        public void HRCommonForkerOnCanOrderWithInvalidOrderByExpectFalse()
        {
            HRSortingParamModel model = new HRSortingParamModel()
            {
                OrderBy = "FIELD1"
            };
            CoreBordersServiceStub service = new CoreBordersServiceStub(null)
            {
                ThrowException = false,
                IsSortable = true
           };
            HRCommonForkerUtils util = new HRCommonForkerUtils();
            Assert.False(util.CanOrder(model, service));
        }
        /// <summary>
        /// Test que CanOrder retourne faux si :
        /// OrderBy est non null
        /// Service est non null
        /// OrderBy est valid
        /// Service is not Sortable
        /// </summary>
        [Fact]
        public void HRCommonForkerOnCanOrderWithUnSortableServiceExpectFalse()
        {
            HRSortingParamModel model = new HRSortingParamModel()
            {
                OrderBy = "FIELD1;ASC"
            };

            CoreBordersServiceStub service = new CoreBordersServiceStub(null)
            {
                ThrowException = false,
                IsSortable = false
            };
            HRCommonForkerUtils util = new HRCommonForkerUtils();
            Assert.False(util.CanOrder(model, service));
        }
        /// <summary>
        /// Test que CanOrder retourne vrai si :
        /// OrderBy est null
        /// Service est non null
        /// </summary>
        [Fact]
        public void HRCommonForkerOnCanOrderWithNullOrEmptyOrderByExpectTrue()
        {
            HRSortingParamModel model = new HRSortingParamModel()
            {
                OrderBy = null
            };

            CoreBordersServiceStub service = new CoreBordersServiceStub(null)
            {
                ThrowException = false,
                IsSortable = true
            };
            HRCommonForkerUtils util = new HRCommonForkerUtils();

            Assert.True(util.CanOrder(model, service));

            model.OrderBy = String.Empty;
            Assert.True(util.CanOrder(model, service));
        }
        /// <summary>
        /// Test que CanOrder retourne faux si :
        /// OrderBy est non null et valide 
        /// Service est null
        /// </summary>
        [Fact]
        public void HRCommonForkerOnCanOrderWithNullServiceAndValidOrderByExpectFalse()
        {
            HRSortingParamModel model = new HRSortingParamModel()
            {
                OrderBy = "FIELD1;ASC"
            };
            HRCommonForkerUtils util = new HRCommonForkerUtils();
            Assert.False(util.CanOrder(model, null));
        }
    }
}
