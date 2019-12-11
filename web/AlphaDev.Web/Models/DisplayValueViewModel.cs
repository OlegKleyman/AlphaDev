namespace AlphaDev.Web.Models
{
    public class DisplayValueViewModel
    {
        public DisplayValueViewModel(string editAction, string controller, string value, string title)
        {
            EditAction = editAction;
            Controller = controller;
            Value = value;
            Title = title;
        }

        public string EditAction { get; set; }

        public string Controller { get; set; }

        public string Value { get; set; }

        public string Title { get; set; }
    }
}