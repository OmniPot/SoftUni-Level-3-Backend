namespace BattleShips.Client.Commands.Models
{
    using Contracts;
    using Enumerations;

    public class PlayCommand : IGameCommand
    {
        public PlayCommand(string gameId, int positionX, int positionY)
        {
            this.GameId = gameId;
            this.PositionX = positionX;
            this.PositionY = positionY;
        }

        public string GameId { get; private set; }

        public int PositionX { get; private set; }

        public int PositionY { get; private set; }

        public GameCommandType CommandType
        {
            get { return GameCommandType.Play; }
        }
    }
}