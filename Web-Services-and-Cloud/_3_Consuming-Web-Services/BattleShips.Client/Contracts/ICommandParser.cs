namespace BattleShips.Client.Contracts
{
    public interface ICommandParser
    {
        IGameCommand ParseCommand(string commandLine);
    }
}