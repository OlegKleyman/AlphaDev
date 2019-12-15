namespace AlphaDev.Web.Core.Support
{
    public class EditorView
    {
        public EditorView(string name, string prefix, string editorElementName)
        {
            Prefix = prefix;
            Name = name;
            EditorElementName = editorElementName;
        }

        public string Prefix { get; }

        public string Name { get; }

        public string EditorElementName { get; }
    }
}