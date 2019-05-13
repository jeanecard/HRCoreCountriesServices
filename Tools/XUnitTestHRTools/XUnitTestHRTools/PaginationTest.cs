using HRCommonTools;
using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTest_PodtGISToNetTopology
{
    public class PaginationTest
    {
        private static IEnumerable<String> Create50Items()
        {
            List<String> paginerItems = new List<String>();
            for (int i = 0; i < 50; i++)
            {
                paginerItems.Add("Items number " + i.ToString());
            }
            return paginerItems;
        }
        //PARTIE VALIDATE
        [Fact]
        void PaginateEmptyListsReturnSinglePagePaginationWithEmptyResult()
        {
            Assert.False(true);
        }
        [Fact]
        void PaginateNullListsReturnSinglePagePaginationWithNullResult()
        {
            Assert.False(true);
        }
        [Fact]
        void PaginateNullPaginationInThrowArgumentsNullException()
        {
            Assert.False(true);
        }
        [Fact]
        void PaginatePaginationInWithPageSize0ReturnPaginationOutWithPageSizeGreaterThan0()
        {
            Assert.False(true);
        }
        [Fact]
        void PaginatePaginationInInvalidThrowAInvalidArgumentException()
        {
            Assert.False(true);
        }
        //PARTIE IS VALID
        [Fact]
        void ValidatePaginationInWithModelNullThrowArgumentNullException()
        {
            InOutPaginer<String> paginer = new InOutPaginer<String>();
            Assert.Throws<ArgumentNullException>(() => paginer.IsValid(null, Create50Items()));
        }
        [Fact]
        void ValidatePaginationInWithEnumerableNullThrowArgumentNullException()
        {
            InOutPaginer<String> paginer = new InOutPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            Assert.Throws<ArgumentNullException>(() => paginer.IsValid(model, null));
        }
        [Fact]
        void ValidatePaginationInPAgeSizeNullThrowInvalidOperationException()
        {
            InOutPaginer<String> paginer = new InOutPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 0;
            Assert.Throws<InvalidOperationException>(() => paginer.IsValid(model, Create50Items()));
        }

        [Fact]
        void ValidatePaginationInOutOfBoundEnumerableThrowArgumentOutOfRangeException()
        {
            InOutPaginer<String> paginer = new InOutPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 2;
            Assert.False(paginer.IsValid(model, Create50Items()));
        }
        [Fact]
        void ValidatePaginationInInRageOfEnumerableReturnTrue()
        {
            Assert.False(true);
        }
        [Fact]
        void ValidatePaginationInFirstPageWithMultiplePagesEnumerableReturnTrue()
        {
            Assert.False(true);
        }
        [Fact]
        void ValidatePaginationInLastPageWithMultiplePagesEnumerableReturnTrue()
        {
            Assert.False(true);
        }
        [Fact]
        void ValidatePaginationInFirstPageWithSinglePageEnumerableReturnTrue()
        {
            Assert.False(true);
        }
    }
 }
