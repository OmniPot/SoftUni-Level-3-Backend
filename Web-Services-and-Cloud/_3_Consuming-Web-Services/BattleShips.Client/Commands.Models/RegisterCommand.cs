namespace BattleShips.Client.Commands.Models
{
    using Contracts;
    using Enumerations;

    public class RegisterCommand : IGameCommand
    {
        public RegisterCommand(string username, string email, string password, string confirmPassword)
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
        }

        public string Username { get; private set; }

        public string Email { get; set; }

        public string Password { get; private set; }

        public string ConfirmPassword { get; private set; }

        public GameCommandType CommandType
        {
            get { return GameCommandType.Register; }
        }
    }
}