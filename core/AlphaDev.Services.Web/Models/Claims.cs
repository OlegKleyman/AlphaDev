namespace AlphaDev.Services.Web.Models
{
    public class Claims
    {
        public Claims(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }

        public string Password { get; }
    }
}