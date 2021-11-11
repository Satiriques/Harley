using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using Interactivity;

namespace SatiriquesBot.Modules.Help
{
    public static class HelpPageBuilder
    {
        public static PageBuilder Build(IEnumerable<CommandInfo> commands, ModuleInfo module)
        {
            return new PageBuilder()
            {
                Title = module.Name,
                Description = string.Join(Environment.NewLine, commands.Select(x => x.Aliases[0]))
            };
        }
    }
}