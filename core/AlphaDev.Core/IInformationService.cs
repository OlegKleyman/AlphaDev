using Optional;

namespace AlphaDev.Core
{
    public interface IInformationService
    {
        Option<string> GetAboutDetails();
        void Edit(string value);
    }
}