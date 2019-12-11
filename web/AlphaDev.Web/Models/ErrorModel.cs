namespace AlphaDev.Web.Models
{
    public struct ErrorModel
    {
        public ErrorModel(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public int Status { get; }

        public string Message { get; }
    }
}