namespace AlphaDev.Services.Web.Models
{
    public class Segmented<T>
    {
        public Segmented(T[] values) => Values = values;

        public T[] Values { get; }

        public int Total { get; set; }
    }
}