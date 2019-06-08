using HRCommonModel;
using HRCommonTools;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTest_PodtGISToNetTopology
{
    public class PaginationTest
    {
        private static IEnumerable<String> CreateItems(int count)
        {
            List<String> paginerItems = new List<String>();
            for (int i = 0; i < count; i++)
            {
                paginerItems.Add("Items number " + i.ToString());
            }
            return paginerItems;
        }
        //PARTIE VALIDATE
        [Fact]
        private void PaginateEmptyListsReturnSinglePagePaginationWithEmptyResult()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageNumber = 0;
            model.PageSize = 20;
            List<String> paginerItems = new List<String>();
            PagingParameterOutModel<String> retour = paginer.GetPagination(model, paginerItems);

            Assert.NotNull(retour);
            Assert.True(retour.TotalItemsCount == 0);
            Assert.True(retour.TotalPages == 1);
            Assert.True(retour.PageSize == 20);
            Assert.NotNull(retour.PageItems);
            int itemsCount = 0;
            IEnumerator<String> enumerator = retour.PageItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                itemsCount++;
            }
            Assert.True(itemsCount == 0);
            Assert.False(retour.HasPreviousPage);
            Assert.False(retour.HasNextPage);
            Assert.True(retour.CurrentPage == 0);
        }
        [Fact]
        private void PaginateNullListsThrowArgumentNullException()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageNumber = 0;
            model.PageSize = 20;
            Assert.Throws<ArgumentNullException>(() => paginer.GetPagination(model, null));
        }
        [Fact]
        private void PaginateNullPaginationInThrowArgumentsNullException()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            Assert.Throws<ArgumentNullException>(() => paginer.GetPagination(null, PaginationTest.CreateItems(50)));

        }
        [Fact]
        private void PaginatePaginationInWithPageSize0ThrowInvalidOperationException()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageNumber = 0;
            model.PageSize = 0;
            Assert.Throws<InvalidOperationException>(() => paginer.GetPagination(model, CreateItems(50)));

        }
        [Fact]
        private void PaginatePaginationInInvalidThrowAInvalidArgumentException()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageNumber = 5;
            model.PageSize = 100;
            Assert.Throws<Exception>(() => paginer.GetPagination(model, CreateItems(50)));
        }
        //PARTIE IS VALID
        [Fact]
        private void ValidatePaginationInWithModelNullThrowArgumentNullException()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            Assert.Throws<ArgumentNullException>(() => paginer.IsValid(null, CreateItems(50)));
        }
        [Fact]
        private void ValidatePaginationInWithEnumerableNullThrowArgumentNullException()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            Assert.Throws<ArgumentNullException>(() => paginer.IsValid(model, null));
        }
        [Fact]
        private void ValidatePaginationInPAgeSizeNullThrowInvalidOperationException()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 0;
            Assert.Throws<InvalidOperationException>(() => paginer.IsValid(model, CreateItems(50)));
        }

        [Fact]
        private void ValidatePaginationInWith50ItemsOutOfBoundEnumerableReturnFalse()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 2;
            Assert.False(paginer.IsValid(model, CreateItems(50)));
        }

        private void ValidatePaginationInWith49ItemsOutOfBoundEnumerableReturnFalse()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 2;
            Assert.False(paginer.IsValid(model, CreateItems(49)));
        }

        [Fact]
        private void ValidatePaginationInInRageOfEnumerableReturnTrue()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 2;
            Assert.True(paginer.IsValid(model, CreateItems(200)));
        }
        [Fact]
        private void ValidatePaginationInFirstPageWithMultiplePagesEnumerableReturnTrue()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 1;
            Assert.True(paginer.IsValid(model, CreateItems(200)));
        }
        [Fact]
        private void ValidatePaginationInLastPageWithMultiplePagesEnumerableReturnTrue()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 3;
            Assert.True(paginer.IsValid(model, CreateItems(200)));
        }
        [Fact]
        private void ValidatePaginationInFirstPageWithSinglePageEnumerableReturnTrue()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 0;
            Assert.True(paginer.IsValid(model, CreateItems(200)));
        }
        //Partie MODEL
        [Fact]
        private void PaginateParameterInSinglePageExpectOutModelHasPreviousAndNextPageFalse()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 0;
            PagingParameterOutModel<String> result = paginer.GetPagination(model, CreateItems(49));
            Assert.NotNull(result);
            Assert.False(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
            Assert.True(result.TotalItemsCount == 49);
            Assert.True(result.CurrentPage == 0);
            IEnumerable<String> items = result.PageItems;
            Assert.NotNull(items);
            Assert.NotEmpty(items);
            IEnumerator<String> enumerator = items.GetEnumerator();
            int i = 0;
            String valueExceptedi = String.Empty;
            while (enumerator.MoveNext())
            {
                valueExceptedi = "Items number " + i.ToString();
                Assert.Equal(valueExceptedi, enumerator.Current);
                i++;
            }
        }

        [Fact]
        private void PaginateMultiplePageOnFirstPageExpectOutModelIsConsistent()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 0;
            PagingParameterOutModel<String> result = paginer.GetPagination(model, CreateItems(200));
            Assert.NotNull(result);
            Assert.True(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
            Assert.True(result.TotalItemsCount == 200);
            Assert.True(result.CurrentPage == 0);
            IEnumerable<String> items = result.PageItems;
            Assert.NotNull(items);
            Assert.NotEmpty(items);
            IEnumerator<String> enumerator = items.GetEnumerator();
            int i = 0;
            String valueExceptedi = String.Empty;
            while (enumerator.MoveNext())
            {
                valueExceptedi = "Items number " + i.ToString();
                Assert.Equal(valueExceptedi, enumerator.Current);
                i++;
            }
        }
        [Fact]
        private void PaginateMultiplePageOnNotSpecificPageExpectOutModelIsConsistent()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 1;
            PagingParameterOutModel<String> result = paginer.GetPagination(model, CreateItems(200));
            Assert.NotNull(result);
            Assert.True(result.HasNextPage);
            Assert.True(result.HasPreviousPage);
            Assert.True(result.TotalItemsCount == 200);
            Assert.True(result.CurrentPage == 1);
            IEnumerable<String> items = result.PageItems;
            Assert.NotNull(items);
            Assert.NotEmpty(items);
            IEnumerator<String> enumerator = items.GetEnumerator();
            int i = 50;
            String valueExceptedi = String.Empty;
            while (enumerator.MoveNext())
            {
                valueExceptedi = "Items number " + i.ToString();
                Assert.Equal(valueExceptedi, enumerator.Current);
                i++;
            }
        }
        [Fact]
        private void PaginateMultiplePageOnLastPageExpectOutModelIsConsistent()
        {
            HRPaginer<String> paginer = new HRPaginer<String>();
            PagingParameterInModel model = new PagingParameterInModel();
            model.PageSize = 50;
            model.PageNumber = 4;
            PagingParameterOutModel<String> result = paginer.GetPagination(model, CreateItems(210));
            Assert.NotNull(result);
            Assert.False(result.HasNextPage);
            Assert.True(result.HasPreviousPage);
            Assert.True(result.TotalItemsCount == 210);
            Assert.True(result.CurrentPage == 4);
            IEnumerable<String> items = result.PageItems;
            Assert.NotNull(items);
            Assert.NotEmpty(items);
            IEnumerator<String> enumerator = items.GetEnumerator();
            int i = 200;
            String valueExceptedi = String.Empty;
            while (enumerator.MoveNext())
            {
                valueExceptedi = "Items number " + i.ToString();
                Assert.Equal(valueExceptedi, enumerator.Current);
                i++;
            }
            Assert.True(i == 210);
        }
    }
}
