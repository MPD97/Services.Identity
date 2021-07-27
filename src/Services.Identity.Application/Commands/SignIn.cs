using Convey.CQRS.Commands;

namespace Services.Identity.Application.Commands
{
    [Contract]
    public class SignIn : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserAgent { get; set; }

        public SignIn(string email, string password, string userAgent)
        {
            Email = email;
            Password = password;
            UserAgent = userAgent;
        }
    }
}