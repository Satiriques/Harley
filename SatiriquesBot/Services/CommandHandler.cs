using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using MtgApiManager.Lib.Service;
using SatiriquesBot.Database.Contexts;
using SatiriquesBot.Database.Controllers;
using Discord.Addons.Interactive;
using Discord;
using System.Text.RegularExpressions;
using System.Linq;
using SatiriquesBot.Modules.Magic;

namespace SatiriquesBot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private IServiceProvider _services;
        private Regex _magicRegex = new Regex(@"\[\[(.*?)\]\]");
        private Regex _hexRegex = new Regex(@"\A\b[0-9a-fA-F]{8}\b\Z");
        public static char Prefix = '\'';
        private ulong _julId = 122138771121635329;
        private ulong _maxId = 126747994795016193;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
            _services = BuildServiceProvider();
        }

        private IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .AddSingleton<CardService>()
            // You can pass in an instance of the desired type
            // ...or by using the generic method.
            //
            // The benefit of using the generic method is that 
            // ASP.NET DI will attempt to inject the required
            // dependencies that are specified under the constructor 
            // for us.
            .AddSingleton<CommandHandler>()
            
            .AddDbContext<UserContext>()
            .AddTransient<UserController>()

            .AddSingleton<InteractiveService>()
            
            .BuildServiceProvider();

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;

            // Here we discover all of the command modules in the entry 
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the 
            // required dependencies.
            //
            // If you do not use Dependency Injection, pass null.
            // See Dependency Injection guide for more information.
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: _services);

            IUserExtensions.Configure(_services.GetService<UserController>());
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
 
            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix(Prefix, ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos) ||
                _magicRegex.IsMatch(message.Content) ||
                _hexRegex.IsMatch(message.Content)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            IResult result = null;

            if (_magicRegex.IsMatch(message.Content))
            {
                var matchResult = _magicRegex.Match(message.Content).Groups[1].Value;
                var searchResult = _commands.Search("mtg");
                var command = searchResult.Commands.FirstOrDefault();
                result = await command.ExecuteAsync(context, new[] { matchResult }, command.Command.Parameters, _services);
            }
            else if (_hexRegex.IsMatch(message.Content))
            {
                // max
                if(context.Message.Author.Id == _maxId)
                {
                    var julUser = context.Guild.GetUser(_julId);
                    await context.Channel.SendMessageAsync("u trya throw sum hands? <:stank:584408549606817809> " + julUser.Mention);
                }
                // moi
                else if(context.Message.Author.Id == _julId)
                {
                    var maxUser = context.Guild.GetUser(_maxId);
                    await context.Channel.SendFileAsync("im-about-to-end.jpg", maxUser.Mention);
                }
            }
            else
            {
                result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
            }

            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
            // Keep in mind that result does not indicate a return value
            // rather an object stating if the command executed successfully.


            // Optionally, we may inform the user if the command fails
            // to be executed; however, this may not always be desired,
            // as it may clog up the request queue should a user spam a
            // command.

        }
    }
}
