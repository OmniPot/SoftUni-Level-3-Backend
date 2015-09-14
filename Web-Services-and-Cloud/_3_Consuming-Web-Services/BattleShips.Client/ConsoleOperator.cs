namespace BattleShips.Client
{
    using System;
    using Contracts;

    public class ConsoleOperator : IConsoleOperator
    {
        private const string ErrorStart = "---- ERROR ----\n";

        public string Read()
        {
            return Console.ReadLine();
        }

        public void Write(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteError(string errorMessage)
        {
            Console.WriteLine(ErrorStart + errorMessage);
        }
    }
}