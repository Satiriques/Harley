using System;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using Interactivity;
using Interactivity.Pagination;
using MoreLinq;
using SatiriquesBot.Attributes;

namespace SatiriquesBot.Modules.Help
{
    [Group("help")]
    [ExcludeFromHelp]
    public class HelpModule : ModuleBase
    {
        private readonly CommandService _commandService;
        private readonly InteractivityService _interactivityService;

        public HelpModule(CommandService service, InteractivityService interactivityService)
        {
            _commandService = service;
            _interactivityService = interactivityService;
        }

        [Command(RunMode = RunMode.Async)]
        [Name("Help")]
        public async Task HelpAsync()
        {
            var commands = _commandService.Commands
                .Where(x => x.Module.Attributes.All(z => z.GetType() != typeof(ExcludeFromHelpAttribute)))
                .Where(x => x.Attributes.All(z => z.GetType() != typeof(ExcludeFromHelpAttribute)))
                .DistinctBy(x => x.Aliases[0])
                .GroupBy(x => x.Module)
                .OrderBy(x => x.Key.Name)
                .ToDictionary(x => x.Key);

            var pages = commands.Select(x => HelpPageBuilder
                .Build(x.Value.OrderBy(y => y.Name), x.Key));

            await _interactivityService.SendPaginatorAsync(
                new StaticPaginatorBuilder() {Pages = pages.ToList()}.Build(), Context.Channel);
        }

        [Command]
        public async Task HelpAsync(string query)
        {
            var commands = _commandService.Commands
                .Where(x => x.Aliases.Any(y => y == query.ToLower()))
                .ToArray();

            if (commands.Any())
            {
                var signatures = commands.Select(x =>
                    $"'{Format.Bold(x.Aliases[0])} " +
                    $"{string.Join(" ", x.Parameters.Select(y => (y.IsOptional ? $"[{y.Name}]" : y.Name)))}" +
                    $"{(string.IsNullOrWhiteSpace(x.Summary) ? "" : Environment.NewLine + x.Summary + Environment.NewLine)}");

                await ReplyAsync(embed: new EmbedBuilder()
                {
                    Title = $"Module {commands[0].Module.Name}, Command {commands[0].Name}",
                    Description = string.Join(Environment.NewLine, signatures),
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = (commands[0].Aliases.Count > 1
                            ? $"Aliases: {string.Join(", ", commands[0].Aliases.Skip(1))}"
                            : ""),
                    }
                }.Build());
            }
        }
    }
}