namespace AlphaDev.Core.Data.Entities
{
    using System;

    public class Blog
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public DateTime? Modified { get; set; }
    }
}