namespace BattleShips.Client.Contracts
{
    public interface IConsoleOperator
    {
        string Read();

        void Write(string textToWrite);

        void WriteError(string serrorTextToWrite);
    }
}