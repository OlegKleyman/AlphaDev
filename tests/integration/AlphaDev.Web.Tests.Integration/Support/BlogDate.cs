using Optional;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class BlogDate
    {
        public BlogDate(string created, Option<string> modified)
        {
            Created = created;
            Modified = modified;
        }

        public string Created { get; }

        public Option<string> Modified { get; }
    }
}