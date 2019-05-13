using System;
using System.Collections.Generic;
using System.Text;

namespace HRCoreBordersModel
{
    public class PagingParameterOutModel<T>
    {
        private int _totalItemsCount = 0;
        private int _pageSize = 1;
        private int _currentPage = 1;
        private int _totalPages = 1;
        private bool _hasPreviousPage = false;
        private bool _hasNextPage = false;
        private IEnumerable<T> _pageItems = null;

        public int TotalItemsCount { get => _totalItemsCount; set => _totalItemsCount = value; }
        public int PageSize { get => _pageSize; set => _pageSize = value; }
        public int CurrentPage { get => _currentPage; set => _currentPage = value; }
        public int TotalPages { get => _totalPages; set => _totalPages = value; }
        public bool HasPreviousPage { get => _hasPreviousPage; set => _hasPreviousPage = value; }
        public bool HasNextPage { get => _hasNextPage; set => _hasNextPage = value; }
        public IEnumerable<T> PageItems { get => _pageItems; set => _pageItems = value; }
        /// <summary>
        /// !TODO
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
