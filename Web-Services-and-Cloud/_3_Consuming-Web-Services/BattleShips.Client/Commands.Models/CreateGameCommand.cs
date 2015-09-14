namespace BattleShips.Client.Commands.Models
{
    using Contracts;
    using Enumerations;

    public class CreateGameCommand : IGameCommand
    {
        public GameCommandType CommandType
        {
            get { return GameCommandType.CreateGame; }
        }
    }
}