using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Core
{
    public class SaveFilterAttribute : TypeFilterAttribute
    {
        public SaveFilterAttribute() : base(typeof(SaveFilter))
        {
        }
    }
}