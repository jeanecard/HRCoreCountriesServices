using System;
using System.Collections.Generic;
using System.Text;

namespace HRCoreBordersModel
{
    public class PagingParameterInModel
    {
        private const int _defaultPageSize = 20;
        private ushort _pageSize = _defaultPageSize;
        private ushort _pageNumber = 1;
        public PagingParameterInModel()
        {
            //Dummy.
        }

        public ushort PageSize
        {
            get => _pageSize; set => _pageSize = value;
        }
        public ushort PageNumber { get => _pageNumber; set => _pageNumber = value; }
    }
}
