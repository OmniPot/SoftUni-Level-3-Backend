namespace BattleShips.Client
{
    using System;
    using Contracts;

    public class GameEngine
    {
        private const string GameWelcomeMessage = "Welcome to the Battleships game!";

        public GameEngine(
            ICommandParser commandParser,
            ICommandExecutor commandExecutor,
            IConsoleOperator consoleOperator)
        {
            this.CommandParser = commandParser;
            this.ConsoleOperator = consoleOperator;
            this.CommandExecutor = commandExecutor;
        }

        private ICommandParser CommandParser { get; set; }
        private ICommandExecutor CommandExecutor { get; set; }
        private IConsoleOperator ConsoleOperator { get; set; }


        public void Run()
        {
            this.ConsoleOperator.Write(GameWelcomeMessage);

            while (true)
            {
                try
                {
                    var commandLine = this.ConsoleOperator.Read();
                    var command = this.CommandParser.ParseCommand(commandLine);
                    this.CommandExecutor.ExecuteCommand(command);
                }
                catch (Exception exception)
                {
                    this.ConsoleOperator.WriteError(exception.Message);
                }
            }
        }
    }
}