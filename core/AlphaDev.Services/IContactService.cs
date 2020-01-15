using System.Threading.Tasks;
using Optional;

namespace AlphaDev.Services
{
    public interface IContactService
    {
        Task<Option<string>> GetContactDetailsAsync();

        Task EditAsync(string value);

        Task CreateAsync(string value);
    }
}