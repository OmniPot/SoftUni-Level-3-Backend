namespace BattleShips.Client.Contracts
{
    using System.Threading.Tasks;

    public interface ICommandExecutor
    {
        IRequester Requester { get; }

        IConsoleOperator ConsoleOperator { get; }

        Task ExecuteCommand(IGameCommand executableCommand);
    }
}