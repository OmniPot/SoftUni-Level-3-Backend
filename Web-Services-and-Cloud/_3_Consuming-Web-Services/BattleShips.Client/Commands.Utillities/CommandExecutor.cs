namespace BattleShips.Client.Commands.Utillities
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Battleships.WebServices.Models;
    using Contracts;
    using Enumerations;
    using Exceptions;
    using Models;

    public class CommandExecutor : ICommandExecutor
    {
        private const string BaseApiEndpoint = "http://localhost:62858/";
        private const string LoginEndpoint = BaseApiEndpoint + "Token";
        private const string RegisterEndpoint = BaseApiEndpoint + "api/account/register";
        private const string AvailableGamesEndpoint = BaseApiEndpoint + "api/games/available";
        private const string CreateGameEndpoint = BaseApiEndpoint + "api/games/create";
        private const string JoinGameEndPoint = BaseApiEndpoint + "api/games/join";
        private const string PlayEndPoint = BaseApiEndpoint + "api/games/play";

        private const string SuccessfullRegistrationMessage = "Successfully registered! You can now log in.";
        private const string SuccessfullLoginMessage = "Successfully logged in! You can now create or join a game.";
        private const string SuccessfullGameCreationMessage = "Successfully created game with Id: ";
        private const string SuccessfullTurnPlayMessage = "Successfully played a turn!";
        private const string PlayersTurnPlayMessage = "Player {0}'s turn!";

        public CommandExecutor(IRequester httpRequester, IConsoleOperator consoleOperator)
        {
            this.ConsoleOperator = consoleOperator;
            this.Requester = httpRequester;
        }

        public IRequester Requester { get; private set; }

        public IConsoleOperator ConsoleOperator { get; private set; }

        // Command executor methods
        public async Task ExecuteCommand(IGameCommand executableCommand)
        {
            try
            {
                switch (executableCommand.CommandType)
                {
                    case GameCommandType.Register:
                        var regResult = await this.ExecuteRegisterCommand(executableCommand as RegisterCommand);
                        this.ConsoleOperator.Write(regResult);
                        break;

                    case GameCommandType.Login:
                        var loginResult = await this.ExecuteLoginCommand(executableCommand as LoginCommand);
                        this.ConsoleOperator.Write(loginResult);
                        break;

                    case GameCommandType.CreateGame:
                        this.Requester.Authenticate();
                        var createdGameResult = await this.ExecuteCreateGameCommand();
                        this.ConsoleOperator.Write(SuccessfullGameCreationMessage + createdGameResult);
                        break;

                    case GameCommandType.AvailableGames:
                        this.Requester.Authenticate();
                        var availableGamesResult = await this.ExecuteAvailableGamesCommand();
                        this.ConsoleOperator.Write(this.BuildAvailableGamesResponse(availableGamesResult));
                        break;

                    case GameCommandType.JoinGame:
                        this.Requester.Authenticate();
                        var joinGameResult = await this.ExecuteJoinGameCommand(executableCommand as JoinGameCommand);
                        this.ConsoleOperator.Write(this.BuildGameField(joinGameResult));
                        break;

                    case GameCommandType.Play:
                        this.Requester.Authenticate();
                        var turnResult = await this.ExecutePlayCommand(executableCommand as PlayCommand);
                        this.ConsoleOperator.Write(SuccessfullTurnPlayMessage);
                        this.ConsoleOperator.Write(this.BuildGameField(turnResult.Field));
                        this.ConsoleOperator.Write(string.Format(PlayersTurnPlayMessage, turnResult.NextPlayerUsername));
                        break;
                }
            }
            catch (InvalidOperationException invalidOperationException)
            {
                this.ConsoleOperator.WriteError(invalidOperationException.Message);
            }
            catch (ApiException apiException)
            {
                foreach (var error in apiException.Errors)
                {
                    this.ConsoleOperator.WriteError(error);
                }
            }
        }

        private async Task<TurnViewModel> ExecutePlayCommand(PlayCommand playCommand)
        {
            var playTurnContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("gameId", playCommand.GameId),
                new KeyValuePair<string, string>("positionX", playCommand.PositionX.ToString()),
                new KeyValuePair<string, string>("positionY", playCommand.PositionY.ToString())
            });

            var response = await this.Requester.HttpClient.PostAsync(PlayEndPoint, playTurnContent);

            if (!response.IsSuccessStatusCode)
            {
                throw ApiException.Create(response);
            }

            return response.Content.ReadAsAsync<TurnViewModel>().Result;
        }

        private async Task<string> ExecuteJoinGameCommand(JoinGameCommand joinGameCommand)
        {
            var joinGameContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("gameId", joinGameCommand.GameId)
            });

            var response = await this.Requester.HttpClient.PostAsync(JoinGameEndPoint, joinGameContent);

            if (!response.IsSuccessStatusCode)
            {
                throw ApiException.Create(response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        private async Task<string> ExecuteCreateGameCommand()
        {
            var response = await this.Requester.HttpClient.PostAsync(CreateGameEndpoint, null);

            if (!response.IsSuccessStatusCode)
            {
                throw ApiException.Create(response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        private async Task<IEnumerable<AvailableGameViewModel>> ExecuteAvailableGamesCommand()
        {
            var response = await this.Requester.HttpClient.GetAsync(AvailableGamesEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw ApiException.Create(response);
            }

            var gameIds = response.Content.ReadAsAsync<IEnumerable<AvailableGameViewModel>>().Result;
            return gameIds;
        }

        private async Task<string> ExecuteLoginCommand(LoginCommand loginCommand)
        {
            var loginContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", loginCommand.Username),
                new KeyValuePair<string, string>("password", loginCommand.Password),
                new KeyValuePair<string, string>("grant_type", "password")
            });

            var response = await this.Requester.HttpClient.PostAsync(LoginEndpoint, loginContent);

            if (!response.IsSuccessStatusCode)
            {
                throw ApiException.Create(response);
            }

            this.Requester.LoggedUserData = response.Content.ReadAsAsync<LoginUserData>().Result;
            this.Requester.SetClientHeaders();

            return SuccessfullLoginMessage;
        }

        private async Task<string> ExecuteRegisterCommand(RegisterCommand registerCommand)
        {
            var registerationContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", registerCommand.Email),
                new KeyValuePair<string, string>("username", registerCommand.Username),
                new KeyValuePair<string, string>("password", registerCommand.Password),
                new KeyValuePair<string, string>("confirmPassword", registerCommand.ConfirmPassword)
            });

            var response = await this.Requester.HttpClient.PostAsync(RegisterEndpoint, registerationContent);

            if (!response.IsSuccessStatusCode)
            {
                throw ApiException.Create(response);
            }

            return SuccessfullRegistrationMessage;
        }

        // Helper methods

        private string BuildGameField(string field)
        {
            var fieldSideLength = (int)Math.Sqrt(field.Length);
            var builder = new StringBuilder();

            builder.AppendLine();
            for (var y = 0; y < fieldSideLength; y++)
            {
                builder.Append("\t");
                for (var x = 0; x < fieldSideLength; x++)
                {
                    builder.Append(field[x + y * fieldSideLength]);
                    builder.Append(" ");
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        private string BuildAvailableGamesResponse(IEnumerable<AvailableGameViewModel> availableGames)
        {
            var builder = new StringBuilder();
            builder.AppendLine("\n----Available games----");
            foreach (var game in availableGames)
            {
                builder.AppendLine("Game Id: " + game.Id);
                builder.AppendLine("First Player: " + game.PlayerOne);
                builder.AppendLine("State: " + game.State);
            }

            return builder.ToString();
        }

    }
}