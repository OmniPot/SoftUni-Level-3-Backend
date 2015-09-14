namespace BattleShips.Client
{
    using System;
    using System.Net.Http;
    using Battleships.WebServices.Models;
    using Contracts;

    public class Requester : IRequester
    {
        private const string UnauthorizedMessage = "Login or register first!";

        public Requester()
        {
            this.HttpClient = new HttpClient();
        }

        public LoginUserData LoggedUserData { get; set; }

        public HttpClient HttpClient { get; private set; }

        public void SetClientHeaders()
        {
            if (this.HttpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                this.HttpClient.DefaultRequestHeaders.Remove("Authorization");
            }

            this.HttpClient.DefaultRequestHeaders.Add(
                "Authorization", "Bearer " + this.LoggedUserData.AccessToken);
        }

        public void Authenticate()
        {
            if (!this.HttpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                throw new InvalidOperationException(UnauthorizedMessage);
            }
        }
    }
}