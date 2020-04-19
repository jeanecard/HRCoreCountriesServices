using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRCoreBordersModel;
using HRCoreBordersRepository;
using System;
using Xunit;

namespace XUnitTestDAL
{
    public class HRCoreBordersRepositoryTest
    {
        PostGISCoreBordersRepository _repo = null;
        public HRCoreBordersRepositoryTest()
        {
            _repo = new PostGISCoreBordersRepository(null, new HRPaginer<HRBorder>());
        }
        /// <summary>
        /// Check that SQL query is used as provided by the class.
        /// </summary>
        [Fact]
        public void PostGISCoreBordersRepository_GetSQLQuery_With_Only_For_Dapper_Param_Set_Expect_Select_Only()
        {
            String sql = _repo.GetSQLQuery(true);
            Assert.NotNull(sql);
            Assert.Equal(sql, PostGISCoreBordersRepository.SQLQUERYFORDAPPER);
        }
        /// <summary>
        /// Check that SQL query is used as provided more the where clause.
        /// </summary>
        [Fact]
        public void PostGISCoreBordersRepository_GetSQLQuery_With_ID_Expect_Select_With_Where_Clause()
        {
            String sql = _repo.GetSQLQuery(true, "XX");
            Assert.NotNull(sql);
            Assert.Equal(sql, PostGISCoreBordersRepository.SQLQUERYFORDAPPER + "WHERE ISO2 = 'XX'");
        }
        /// <summary>
        /// Check that SQL query is used as provided more the order by clause.
        /// </summary>
        [Fact]
        public void PostGISCoreBordersRepository_GetSQLQuery_With_Valid_OrderBy_Expect_Select_With_Order_By_Clause()
        {
            String sql = _repo.GetSQLQuery(true, null, new HRCommonModels.HRSortingParamModel() { OrderBy = "name;asc" }, null);
            Assert.NotNull(sql);
            Assert.Equal(sql, PostGISCoreBordersRepository.SQLQUERYFORDAPPER + " ORDER BY name ASC ");
        }
        /// <summary>
        /// Check that SQL query is used as provided more the Pagination clause and the default order by.
        /// </summary>
        [Fact]
        public void PostGISCoreBordersRepository_GetSQLQuery_With_Valid_OrderBy_Expect_Select_With_Pagination_Clause()
        {
            String sql = _repo.GetSQLQuery(true,
                null,
                null,
                new PagingParameterInModel() { PageNumber = 0, PageSize = 20 });
            Assert.NotNull(sql);
            Assert.Equal(sql, PostGISCoreBordersRepository.SQLQUERYFORDAPPER + " ORDER BY ISO2 ASC  LIMIT 20 OFFSET 0 ");
        }
        /// <summary>
        /// Check that SQL query is used as provided more the Pagination clause and the given order by 
        /// </summary>
        [Fact]
        public void PostGISCoreBordersRepository_GetSQLQuery_With_Valid_Order_By_Expect_Select_With_Pagination_And_Order_By_Clause()
        {
            String sql = _repo.GetSQLQuery(true,
                null,
                new HRCommonModels.HRSortingParamModel() { OrderBy = "name;desc" },
                new PagingParameterInModel() { PageNumber = 0, PageSize = 20 });
            Assert.NotNull(sql);
            Assert.Equal(sql, PostGISCoreBordersRepository.SQLQUERYFORDAPPER + " ORDER BY name DESC  LIMIT 20 OFFSET 0 ");
        }
        /// <summary>
        /// Check that SQL query throw InvalidOperationException when order by contains unknown Fields 
        /// </summary>
        [Fact]
        public void PostGISCoreBordersRepository_GetSQLQueryWith_InValid_Order_By_Throw_Invalid_Operation_Exception()
        {
            Assert.Throws<InvalidOperationException>(() =>
            _repo.GetSQLQuery(
                true,
                null,
                new HRSortingParamModel() { OrderBy = "nameXXXX;ASC" },
                new PagingParameterInModel() { PageNumber = 0, PageSize = 20 }));
        }
    }
}
