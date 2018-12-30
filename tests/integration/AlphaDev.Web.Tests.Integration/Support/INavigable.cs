namespace AlphaDev.Web.Tests.Integration.Support
{
    public interface INavigable<out T> where T : WebPage
    {
        T GoTo();
    }
}