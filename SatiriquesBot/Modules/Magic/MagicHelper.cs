

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Discord;
using MtgApiManager.Lib.Core;
using System.Linq;
using MtgApiManager.Lib.Model;
using SatiriquesBot.Common;

namespace SatiriquesBot.Modules.Magic
{
    public static class MagicHelper
    {
        private static IReadOnlyCollection<Emote> _emotes;
        private static string _prefix = "mtg_"; 
        public static void UseEmojis(IReadOnlyCollection<Emote> emotes)
        {
            _emotes = emotes;
        }

        private static Dictionary<string, uint> magicColors = new Dictionary<string, uint>
        {
            { "Black", 0x000000 },
            { "White", 0xFFFFFF },
            { "Red", 0xFF0000 },
            { "Green", 0x00FF00 },
            { "Blue", 0x0000FF },
        };
        internal static Embed BuildEmbed(Card card)
        {
            return new EmbedBuilder()
            {
                Title = card.Name + TranslateMagicString(card.ManaCost),
                Description = BuildDescription(card.Text, card.Flavor),
                ThumbnailUrl = card.ImageUrl?.ToString(),
                Color = GetColor(card.Colors),
                Footer = new EmbedFooterBuilder()
                {
                    Text = card.Artist,
                }
            }.Build();
        }

        private static string TranslateMagicString(string magicString)
        {
            if (string.IsNullOrWhiteSpace(magicString))
                return magicString;
            Regex regex = new Regex("{(\\w+)}");
            var matches = regex.Matches(magicString);

            foreach (Match match in matches)
            {
                if (_emotes.Any(x => x.Name == _prefix+match.Groups[1].Value))
                {
                    var emote = _emotes.Single(x => x.Name == _prefix + match.Groups[1].Value);
                    var emoteString = $"<:{emote.Name}:{emote.Id}>";
                    magicString = magicString.Replace(match.Groups[0].Value, emoteString);
                }
            }

            return magicString;
        }

        private static string BuildDescription(string cardText, string cardFlavor)
        {
            string description = "";
            if (!string.IsNullOrWhiteSpace(cardText))
                description += Style.Bold(TranslateMagicString(cardText)) + Environment.NewLine;
            description += Style.Italics(cardFlavor);
            return description;
        }

        private static Color GetColor(string[] colors)
        {
            if(colors.Length == 0) return Color.LightGrey;
            return colors.Length > 1 ? Discord.Color.Gold : new Color(magicColors[colors[0]]);
        }

    }
}
