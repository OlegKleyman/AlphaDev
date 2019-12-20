using System.Threading.Tasks;
using Optional;

namespace AlphaDev.Core
{
    public interface IAboutService
    {
        Task<Option<string>> GetAboutDetailsAsync();

        Task EditAsync(string value);

        Task CreateAsync(string value);
    }
}