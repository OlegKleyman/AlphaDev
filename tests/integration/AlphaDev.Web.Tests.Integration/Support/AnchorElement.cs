namespace AlphaDev.Web.Tests.Integration.Support
{
    public class AnchorElement
    {
        public string Text { get; }
        public string Href { get; }

        public AnchorElement(string text, string href)
        {
            Text = text;
            Href = href;
        }
    }
}