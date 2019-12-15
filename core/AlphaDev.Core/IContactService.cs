using Optional;

namespace AlphaDev.Core
{
    public interface IContactService
    {
        Option<string> GetContactDetails();

        void Edit(string value);

        void Create(string value);
    }
}