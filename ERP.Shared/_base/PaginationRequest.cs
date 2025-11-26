namespace ERP.Shared._base
{
    /// <summary>
    /// Generic pagination request model for all list queries
    /// </summary>
    public class PaginationRequest
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;

        /// <summary>
        /// Current page number (starts from 1)
        /// </summary>
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : (value > 10000 ? 10000 : value);
        }

        /// <summary>
        /// Optional search term for filtering
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Calculate skip count for database query
        /// </summary>
        public int Skip => (PageNumber - 1) * PageSize;

        public PaginationRequest()
        {
        }

        public PaginationRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
