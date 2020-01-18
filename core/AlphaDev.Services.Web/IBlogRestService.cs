using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaDev.Services.Web.Models;
using Refit;

namespace AlphaDev.Services.Web
{
    public interface IBlogRestService
    {
        [Get("/blog/GetLatest")]
        Task<ApiResponse<Blog>> GetLatest();

        [Get("/blog/{id}")]
        Task<ApiResponse<Blog>> Get(int id);

        [Post("/blog")]
        Task<ApiResponse<Blog>> Add([Body] Blog blog);

        [Delete("/blog/{id}")]
        Task<HttpResponseMessage> Delete(int id);

        [Put("/blog/{id}")]
        Task<HttpResponseMessage> Edit(int id, [Body] Blog blog);

        [Get("/blog/start/{start}/end/{end}")]
        Task<Segmented<Blog>> Get(int start, int count);
    }
}