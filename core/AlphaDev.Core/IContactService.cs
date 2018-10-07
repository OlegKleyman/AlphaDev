using Optional;

namespace AlphaDev.Core
{
    public interface IContactService
    {
        Option<string> GetDetails();
    }
}