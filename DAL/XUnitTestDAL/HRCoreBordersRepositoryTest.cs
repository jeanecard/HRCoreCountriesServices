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
        /// Check that SQL query is used as provided more the where clause by the class.
        /// </summary>
        [Fact]
        public void GetSQLQueryWithIDExpectSelectWithWherreClause()
        {
            PostGISCoreBordersRepository repo = new PostGISCoreBordersRepository(null);
            String sql = repo.GetSQLQuery(true, "XX");
            Assert.NotNull(sql);
            Assert.Equal(sql, PostGISCoreBordersRepository.SQLQUERYFORDAPPER + "WHERE FIPS = 'XX'");
        }
    }
}
