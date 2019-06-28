using HRCommonModel;
using HRCommonModels;
using HRCoreBordersRepository;
using System;
using Xunit;

namespace XUnitTestDAL
{
    public class HRCoreBordersRepositoryTest
    {
        /// <summary>
        /// Check that SQL query is used as provided by the class.
        /// </summary>
        [Fact]
        public void GetSQLQueryWithNoIDExpectSelectOnly()
        {
            PostGISCoreBordersRepository repo = new PostGISCoreBordersRepository(null);
            String sql = repo.GetSQLQuery(true);
            Assert.NotNull(sql);
            Assert.Equal(sql, PostGISCoreBordersRepository.SQLQUERYFORDAPPER);
        }
        /// <summary>
        /// Check that SQL query is used as provided more the where clause.
        /// </summary>
        [Fact]
        public void GetSQLQueryWithIDExpectSelectWithWherreClause()
        {
            PostGISCoreBordersRepository repo = new PostGISCoreBordersRepository(null);
            String sql = repo.GetSQLQuery(true, "XX");
            Assert.NotNull(sql);
            Assert.Equal(sql, PostGISCoreBordersRepository.SQLQUERYFORDAPPER + "WHERE FIPS = 'XX'");
        }
        /// <summary>
        /// Check that SQL query is used as provided more the order by clause.
        /// </summary>
        [Fact]
        public void GetSQLQueryWithValidOrderByExpectSelectWithOrderByClause()
        {
            PostGISCoreBordersRepository repo = new PostGISCoreBordersRepository(null);
            String sql = repo.GetSQLQuery(true, null, new HRCommonModels.HRSortingParamModel() { SortingParamsQuery = "name;asc" }, null);
            Assert.NotNull(sql);
            Assert.Equal(sql, PostGISCoreBordersRepository.SQLQUERYFORDAPPER + " ORDER BY name ASC ");
        }
        /// <summary>
        /// Check that SQL query is used as provided more the Pagination clause and the default order by.
        /// </summary>
        [Fact]
        public void GetSQLQueryWithValidOrderByExpectSelectWithPaginationClause()
        {
            PostGISCoreBordersRepository repo = new PostGISCoreBordersRepository(null);
            String sql = repo.GetSQLQuery(true,
                null,
                null,
                new PagingParameterInModel() { PageNumber = 0, PageSize = 20});
            Assert.NotNull(sql);
            Assert.Equal(sql, PostGISCoreBordersRepository.SQLQUERYFORDAPPER + " ORDER BY FIPS ASC  LIMIT 20 OFFSET 0 ");
        }
        /// <summary>
        /// Check that SQL query is used as provided more the Pagination clause and the given order by 
        /// </summary>
        [Fact]
        public void GetSQLQueryWithValidOrderByExpectSelectWithPaginationAndOrderByClause()
        {
            PostGISCoreBordersRepository repo = new PostGISCoreBordersRepository(null);
            String sql = repo.GetSQLQuery(true,
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
        public void GetSQLQueryWithInValidOrderByThrowInvalidOperationException()
        {
            PostGISCoreBordersRepository repo = new PostGISCoreBordersRepository(null);
            Assert.Throws<InvalidOperationException>(() => 
            repo.GetSQLQuery(
                true, 
                null, 
                new HRSortingParamModel() { OrderBy="nameXXXX;ASC"}, 
                new PagingParameterInModel() { PageNumber = 0, PageSize = 20 }));
        }
    }
}
