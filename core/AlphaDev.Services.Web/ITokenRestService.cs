using System.Threading.Tasks;
using AlphaDev.Services.Web.Models;
using Refit;

namespace AlphaDev.Services.Web
{
    public interface ITokenRestService
    {
        [Post("/token")]
        Task<string> GetToken([Body] Claims claims);
    }
}
