namespace BattleShips.Client.Commands.Models
{
    using Contracts;
    using Enumerations;

    public class JoinGameCommand : IGameCommand
    {
        public JoinGameCommand(string gameId)
        {
            this.GameId = gameId;
        }

        public string GameId { get; private set; }

        public GameCommandType CommandType
        {
            get { return GameCommandType.JoinGame; }
        }
    }
}