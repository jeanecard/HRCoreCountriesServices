using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTest_PodtGISToNetTopology
{
    public class PaginationTest
    {
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
        void ValidatePaginationInOutOfBoundEnumerableThrowArgumentOutOfRangeException()
        {
            Assert.False(true);
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
