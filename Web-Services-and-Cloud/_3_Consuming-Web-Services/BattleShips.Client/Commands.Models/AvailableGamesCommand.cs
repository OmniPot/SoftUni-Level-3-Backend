namespace BattleShips.Client.Commands.Models
{
    using Contracts;
    using Enumerations;

    public class AvailableGamesCommand : IGameCommand
    {
        public GameCommandType CommandType
        {
            get { return GameCommandType.AvailableGames; }
        }
    }
}