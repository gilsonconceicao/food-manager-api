namespace FoodManager.Domain.Extensions
{
    public class PagedList<T>
    {
        public PagedList(List<T> data, double count, int pageNumber, int pageSize)
        {
            Data = data;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Size = pageSize;
            Page = pageNumber;
            TotalItems = count;
        }

        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
        public double TotalItems { get; set; }

    }
}