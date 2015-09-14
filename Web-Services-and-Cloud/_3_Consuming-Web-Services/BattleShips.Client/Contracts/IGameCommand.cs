namespace BattleShips.Client.Contracts
{
    using Enumerations;

    public interface IGameCommand
    {
        GameCommandType CommandType { get; }
    }
}