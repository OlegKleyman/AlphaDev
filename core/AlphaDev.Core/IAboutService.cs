using Optional;

namespace AlphaDev.Core
{
    public interface IAboutService
    {
        Option<string> GetAboutDetails();
        void Edit(string value);
        void Create(string value);
    }
}