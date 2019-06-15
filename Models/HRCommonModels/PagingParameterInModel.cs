namespace HRCommonModel
{
    /// <summary>
    /// Input parameter to query a Pagination.
    /// Prefered custom one to top, skip, count standard.
    /// In superior version a proxy can be done over this object with the classical top, skip, count server pagination.
    /// </summary>
    public class PagingParameterInModel
    {
        //Arbitrary value to init.
        private ushort _pageSize = 20;
        private ushort _pageNumber = 0;
        /// <summary>
        /// Dummy constructor.
        /// </summary>
        public PagingParameterInModel()
        {
            //Dummy.
        }
        /// <summary>
        /// The number of items (capacity) per page.
        /// </summary>
        public ushort PageSize
        {
            get => _pageSize; set => _pageSize = value;
        }
        /// <summary>
        /// The number of page, 0 based index.
        /// </summary>
        public ushort PageNumber { get => _pageNumber; set => _pageNumber = value; }
    }
}
