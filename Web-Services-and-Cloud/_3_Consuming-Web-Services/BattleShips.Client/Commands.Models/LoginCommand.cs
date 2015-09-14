namespace BattleShips.Client.Commands.Models
{
    using Contracts;
    using Enumerations;

    public class LoginCommand : IGameCommand
    {
        public LoginCommand(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public GameCommandType CommandType
        {
            get { return GameCommandType.Login; }
        }
    }
}