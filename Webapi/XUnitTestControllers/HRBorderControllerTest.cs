using System;
using Xunit;

namespace XUnitTestControllers
{
    public class HRBorderControllerTest
    {
        #region GetByID
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void HRBorderOnGetByIDUnknownExpectStatusCode404()
        {
            Assert.True(false);
        }
        [Fact]
        public void HRBorderOnGetByIDNullExpectStatus400BadRequest()
        {
            Assert.True(false);

        }
        [Fact]
        public void HRBorderOnGetByIDWithNumServiceExpectStatus500InternalServerError()
        {
            Assert.True(false);

        }
        [Fact]
        public void HRBorderOnGetByIDWithExceptionThrownByServiceExpectStatus500InternalServerError()
        {
            Assert.True(false);

        }
        [Fact]
        public void HRBorderOnGetByIDWithExistingItemExpectItemAndCodeStatus200()
        {
            Assert.True(false);

        }
        #endregion

        #region GetAll
        [Fact]
        public void HRBorderGetAllWithInvalidPagingInExpectStatus416RequestedRangeNotSatisfiable()
        {
            Assert.True(false);
        }
        [Fact]
        public void HRBorderOnGetAllWithExceptionThrownByServiceExpectStatus500InternalServerError()
        {
            Assert.True(false);

        }
        [Fact]
        public void HRBorderOnGetAllWithValidPagingInExpectItemsAndCodeStatus200()
        {
            Assert.True(false);

        }
        #endregion
    }
}
