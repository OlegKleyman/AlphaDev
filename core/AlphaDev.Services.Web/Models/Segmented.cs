namespace AlphaDev.Services.Web.Models
{
    public class Segmented<T>
    {
        public T[]? Values { get; set; }

        public int Total { get; set; }
    }
}