namespace AlphaDev.Web.Tests.Integration.Support
{
    public interface INavigable<out T>
    {
        T GoTo();
    }
}