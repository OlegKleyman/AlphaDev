using Optional;

namespace AlphaDev.Core
{
    public interface IContactService
    {
        Option<string> GetDetails();
        void Edit(string value);
        void Create(string value);
    }
}