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
            CoreBordersRepository repo = new CoreBordersRepository(null);
            String sql = repo.GetSQLQuery(true);
            Assert.NotNull(sql);
            Assert.Equal(sql, CoreBordersRepository.SQLQUERYFORDAPPER);
        }
        /// <summary>
        /// Check that SQL query is used as provided more the where clause by the class.
        /// </summary>
        [Fact]
        public void GetSQLQueryWithIDExpectSelectWithWherreClause()
        {
            CoreBordersRepository repo = new CoreBordersRepository(null);
            String sql = repo.GetSQLQuery(true, "XX");
            Assert.NotNull(sql);
            Assert.Equal(sql, CoreBordersRepository.SQLQUERYFORDAPPER + "WHERE FIPS = 'XX'");
        }
    }
}
