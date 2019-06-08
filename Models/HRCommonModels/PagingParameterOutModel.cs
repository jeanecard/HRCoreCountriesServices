using System;
using System.Collections.Generic;

namespace HRCommonModel
{
    /// <summary>
    /// Output resyult of a Pagination
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingParameterOutModel<T>
    {
        private int _totalItemsCount = 0;
        private int _pageSize = 1;
        private int _currentPage = 1;
        private int _totalPages = 1;
        private IEnumerable<T> _pageItems = null;
        /// <summary>
        /// Total items count in the query
        /// </summary>
        public int TotalItemsCount { get => _totalItemsCount; set => _totalItemsCount = value; }
        /// <summary>
        /// Max Number of items in a page
        /// </summary>
        public int PageSize { get => _pageSize; set => _pageSize = value; }
        /// <summary>
        /// The index (0 based) of the current page
        /// </summary>
        public int CurrentPage { get => _currentPage; set => _currentPage = value; }
        /// <summary>
        /// The number of Page in the query
        /// </summary>
        public int TotalPages { get => _totalPages; set => _totalPages = value; }
        /// <summary>
        /// Return true if current page has a previous page
        /// </summary>
        public bool HasPreviousPage { get => _currentPage > 0; }
        /// <summary>
        /// Return true if current page has a next page
        /// </summary>
        public bool HasNextPage { get => _currentPage + 1  < TotalPages;  }
        /// <summary>
        /// Return the items in the page.
        /// </summary>
        public IEnumerable<T> PageItems { get => _pageItems; set => _pageItems = value; }
    }
}
