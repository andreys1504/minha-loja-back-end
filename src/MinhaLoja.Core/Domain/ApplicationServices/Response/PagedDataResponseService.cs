namespace MinhaLoja.Core.Domain.ApplicationServices.Response
{
    public class PagedDataResponseService
    {
        protected PagedDataResponseService()
        {
        }

        public PagedDataResponseService(
            int currentPage,
            int pageSize,
            int total)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalItems = total;
        }

        public int CurrentPage { get; set; }
        public double QuantityPages
        {
            get { return (double)TotalItems / PageSize; }
        }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}
