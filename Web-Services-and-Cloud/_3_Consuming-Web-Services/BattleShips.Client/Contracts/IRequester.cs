namespace BattleShips.Client.Contracts
{
    using System.Net.Http;
    using Battleships.WebServices.Models;

    public interface IRequester
    {
        HttpClient HttpClient { get; }

        LoginUserData LoggedUserData { get; set; }

        void Authenticate();

        void SetClientHeaders();
    }
}