using System.Threading.Tasks;
using AlphaDev.Core;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaDev.Web.Core
{
    public class SaveFilter : IAsyncActionFilter
    {
        private readonly ISaveToken _token;

        public SaveFilter(ISaveToken token) => _token = token;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            if (result.Exception is null || result.ExceptionHandled)
            {
                await _token.SaveAsync();
            }
        }
    }
}
