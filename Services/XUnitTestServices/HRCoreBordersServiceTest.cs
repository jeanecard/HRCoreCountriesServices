using HRCoreBordersModel;
using HRCoreBordersServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using XUnitTestServices.MocksAndStubs;

namespace XUnitTestServices
{
    public class HRCoreBordersServiceTest
    {
        /// <summary>
        /// Test that MemberAccessException is thrown on GetBordersAsync with a null repoistory.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncThrowMemberAccessExceptionIfRepositoryIsNullExpectTrue()
        {
            HRCoreBordersService service = new HRCoreBordersService(null);
            bool exceptionThrown = false;
            try
            {
                Task<IEnumerable<HRBorder>>  retour = service.GetBordersAsync("xx");
                await retour;
            }
            catch(MemberAccessException)
            {
                exceptionThrown = true;
            }
            catch(Exception)
            {
                //Dummy.
            }
            Assert.True(exceptionThrown);
        }
        /// <summary>
        /// Verify that Service do not do any extra processing on repository results.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncWithBorderIDReturnRepositoryResultWithoutExtraProcessingwExpectTrue()
        {
            HRCoreBordersRepositoryStub repository = new HRCoreBordersRepositoryStub(null, "XX");
            HRCoreBordersService service = new HRCoreBordersService(repository);
            Task<IEnumerable<HRBorder>> repoResult = service.GetBordersAsync("XX");
            await repoResult;
            Assert.NotNull(repoResult.Result);
            IEnumerator<HRBorder> enumerator = repoResult.Result.GetEnumerator();
            enumerator.MoveNext();
            Assert.NotNull(enumerator.Current);
            Assert.True(enumerator.Current.FIPS == "XX");
        }

        /// <summary>
        /// Verify that Service do not do any extra processing on repository results.
        /// </summary>
        [Fact]
        public async void BorderServiceOnGetBordersAsyncWithoutBorderIDReturnRepositoryResultWithoutExtraProcessingwExpectTrue()
        {
            List<String> bordersID = new List<string>() { "1", "2", "3" };
            HRCoreBordersRepositoryStub repository = new HRCoreBordersRepositoryStub(bordersID, null);
            HRCoreBordersService service = new HRCoreBordersService(repository);
            Task<IEnumerable<HRBorder>> repoResult = service.GetBordersAsync(null);
            await repoResult;
            Assert.NotNull(repoResult.Result);
            IEnumerator<HRBorder> enumerator = repoResult.Result.GetEnumerator();
            int i = 1;
            while (enumerator.MoveNext())
            {
                Assert.NotNull(enumerator.Current);
                Assert.True(enumerator.Current.FIPS == i.ToString());
                i++;
            }
            Assert.True(i == 4);
        }
    }
}
