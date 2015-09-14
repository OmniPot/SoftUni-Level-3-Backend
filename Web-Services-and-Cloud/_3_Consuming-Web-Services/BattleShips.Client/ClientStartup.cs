namespace BattleShips.Client
{
    using Commands.Utillities;

    public class ClientStartup
    {
        public static void Main()
        {
            var consoleOperator = new ConsoleOperator();
            var httpRequester = new Requester();
            var commandParser = new CommandParser();

            var commandExecutor = new CommandExecutor(httpRequester, consoleOperator);
            var gameEngine = new GameEngine(commandParser, commandExecutor, consoleOperator);

            gameEngine.Run();
        }
    }
}