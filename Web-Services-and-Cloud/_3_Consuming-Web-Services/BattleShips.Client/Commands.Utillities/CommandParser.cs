namespace BattleShips.Client.Commands.Utillities
{
    using System;
    using System.ComponentModel;
    using Contracts;
    using Models;

    public class CommandParser : ICommandParser
    {
        private const string RegisterCommandKeyword = "register";
        private const string LoginCommandKeyword = "login";

        private const string CreateGameCommandKeyword = "create-game";
        private const string AvailableGamesKeyword = "available";
        private const string JoinGameCommandKeyword = "join-game";
        private const string PlayCommandKeyword = "play";

        private const int PlayCommandParametersCount = 4;
        private const int JoinGameCommandParametersCount = 2;
        private const int LoginCommandParametersCount = 3;
        private const int RegisterCommandParametersCount = 5;
        private const int UsernameMinLength = 5;
        private const int EmailMinLength = 6;
        private const int GameIdLength = 36;

        private const string InvalidCommandErrorMessage = "Invalid command!";

        private const string PlayCommandParametersCountErrorMessage =
            "Invalid command parameters! Try: $ play gameId x y";

        private const string PlayCommandInvalidCoordinateErrorMessage =
            "The PositionX and PositionY coordinates must be in the range [1...8].";

        private const string PlayCommandInvalidGameIdErrorMessage =
            "The game id must contain 32 characters and 4 dashes.";

        private const string JoinGameCommandParametersCountErrorMessage =
            "Invalid command parameters! Try: $ join-game gameId";

        private const string LoginCommandParametersCountErrorMessage =
            "Invalid command parameters! Try: $ login username password";

        private const string RegisterCommandParametersCountErrorMessage =
            "Invalid command parameters! Try: $ register username email password confirmPassword";

        private const string NullOrEmptyCommand = "Command cannot be null or empty string.";
        private const string UsernameLengthErrorMessage = "Username should be at least 5 symbols long.";
        private const string EmailLengthErrorMessage = "Email should be at least 6 symbols long.";
        private const string ConfirmPasswordMismatchErrorMessage = "Password does not match the confirmation.";

        public IGameCommand ParseCommand(string commandLine)
        {
            if (commandLine.Equals(string.Empty) || commandLine == null)
            {
                throw new ArgumentNullException("commandLine", NullOrEmptyCommand);
            }

            var commandComponenets = commandLine.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var commandKeyWord = commandComponenets[0];
            IGameCommand commandResult;

            switch (commandKeyWord)
            {
                case RegisterCommandKeyword:
                    commandResult = this.CreateRegisterCommand(commandComponenets);
                    break;
                case LoginCommandKeyword:
                    commandResult = this.CreateLoginCommand(commandComponenets);
                    break;
                case AvailableGamesKeyword:
                    commandResult = this.CreateAvailableGamesCommand();
                    break;
                case CreateGameCommandKeyword:
                    commandResult = this.CreateCreateGameCommand();
                    break;
                case JoinGameCommandKeyword:
                    commandResult = this.CreateJoinGameCommand(commandComponenets);
                    break;
                case PlayCommandKeyword:
                    commandResult = this.CreatePlayCommand(commandComponenets);
                    break;
                default:
                    throw new InvalidEnumArgumentException(InvalidCommandErrorMessage);
            }

            return commandResult;
        }

        private PlayCommand CreatePlayCommand(string[] commandComponenets)
        {
            if (commandComponenets.Length != PlayCommandParametersCount)
            {
                throw new InvalidOperationException(PlayCommandParametersCountErrorMessage);
            }

            var gameId = commandComponenets[1];
            if (gameId.Length != GameIdLength)
            {
                throw new ArgumentOutOfRangeException(PlayCommandInvalidGameIdErrorMessage);
            }

            var positionX = int.Parse(commandComponenets[2]);
            if (positionX > 8 || positionX < 1)
            {
                throw new ArgumentOutOfRangeException(PlayCommandInvalidCoordinateErrorMessage);
            }

            var positionY = int.Parse(commandComponenets[3]);
            if (positionY > 8 || positionY < 1)
            {
                throw new ArgumentOutOfRangeException(PlayCommandInvalidCoordinateErrorMessage);
            }

            return new PlayCommand(gameId, positionX - 1, positionY - 1);
        }

        private JoinGameCommand CreateJoinGameCommand(string[] commandComponenets)
        {
            if (commandComponenets.Length != JoinGameCommandParametersCount)
            {
                throw new InvalidOperationException(JoinGameCommandParametersCountErrorMessage);
            }

            var gameId = commandComponenets[1];
            var command = new JoinGameCommand(gameId);

            return command;
        }

        private CreateGameCommand CreateCreateGameCommand()
        {
            return new CreateGameCommand();
        }

        private AvailableGamesCommand CreateAvailableGamesCommand()
        {
            return new AvailableGamesCommand();
        }

        private LoginCommand CreateLoginCommand(string[] commandComponenets)
        {
            if (commandComponenets.Length != LoginCommandParametersCount)
            {
                throw new InvalidOperationException(LoginCommandParametersCountErrorMessage);
            }

            var username = commandComponenets[1];
            if (username.Length < UsernameMinLength)
            {
                throw new ArgumentOutOfRangeException(UsernameLengthErrorMessage);
            }

            var password = commandComponenets[2];
            var command = new LoginCommand(username, password);

            return command;
        }

        private RegisterCommand CreateRegisterCommand(string[] commandComponenets)
        {
            if (commandComponenets.Length != RegisterCommandParametersCount)
            {
                throw new InvalidOperationException(RegisterCommandParametersCountErrorMessage);
            }

            var username = commandComponenets[1];
            if (username.Length < UsernameMinLength)
            {
                throw new ArgumentOutOfRangeException(UsernameLengthErrorMessage);
            }

            var email = commandComponenets[2];
            if (email.Length < EmailMinLength)
            {
                throw new ArgumentOutOfRangeException(EmailLengthErrorMessage);
            }

            var password = commandComponenets[3];
            var confirmPassword = commandComponenets[4];
            if (!password.Equals(confirmPassword))
            {
                throw new ArgumentOutOfRangeException(ConfirmPasswordMismatchErrorMessage);
            }

            var command = new RegisterCommand(username, email, password, confirmPassword);

            return command;
        }
    }
}