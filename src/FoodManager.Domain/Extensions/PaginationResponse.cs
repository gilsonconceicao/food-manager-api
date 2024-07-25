namespace FoodManager.Domain.Extensions
{
    public class PagedList<T>
    {
        public PagedList(List<T> data, double count, int pageNumber, int pageSize)
        {
            Data = data;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
        }

        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public double TotalCount { get; set; }

    }
}