namespace JobPortal.CQRS.Common.Models
{
    public abstract class PagedQuery
    {
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 50;
        
        private int _pageSize = DefaultPageSize;
        private int _pageNumber = 1;
        
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }
        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? DefaultPageSize : value);
        }
        
        public string SearchTerm { get; set; }
        
        public string SortColumn { get; set; }
        
        public string SortDirection { get; set; } = "asc";
    }
} 