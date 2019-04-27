

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Discord;
using MtgApiManager.Lib.Core;
using System.Linq;
using MtgApiManager.Lib.Model;
using SatiriquesBot.Common;
using static Discord.Addons.Interactive.PaginatedMessage;

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
            { "Black", 0x150b00 },
            { "White", 0xf9faf4 },
            { "Red", 0xd3202b },
            { "Green", 0x00733e },
            { "Blue", 0x0e68ab },
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

        internal static Page BuildPage(Card card, int index, int numberOfPages)
        {
            return new Page()
            {
                Title = card.Name + TranslateMagicString(card.ManaCost),
                Description = BuildDescription(card.Text, card.Flavor, card.Power, card.Toughness),
                ThumbnailUrl = card.ImageUrl?.ToString(),
                Color = GetColor(card.Colors),
                FooterOverride = new EmbedFooterBuilder()
                {
                    Text = $"Page {index+1}/{numberOfPages} — Illus. {card.Artist}",
                }
            };
        }

        private static string TranslateMagicString(string magicString)
        {
            if (string.IsNullOrWhiteSpace(magicString))
                return magicString;
            
            // match everything between brackets, lazy
            Regex regex = new Regex("{(.*?)}");
            var matches = regex.Matches(magicString);

            foreach (Match match in matches)
            {
                if (_emotes.Any(x => x.Name == _prefix+match.Groups[1].Value.Replace("/", "")))
                {
                    var emote = _emotes.Single(x => x.Name == _prefix + match.Groups[1].Value.Replace("/",""));
                    var emoteString = $"<:{emote.Name}:{emote.Id}>";
                    magicString = magicString.Replace(match.Groups[0].Value, emoteString);
                }
            }

            return magicString;
        }

        private static string BuildDescription(string cardText, string cardFlavor, string power=null, string toughtness=null)
        {
            string description = "";
            if (!string.IsNullOrWhiteSpace(cardText))
                description += Style.Bold(TranslateMagicString(cardText)) + Environment.NewLine;
            description += Style.Italics(cardFlavor);

            if (!string.IsNullOrWhiteSpace(power))
                description += Environment.NewLine + Environment.NewLine + Format.Bold(power + "/" + toughtness);

            return description;
        }

        private static Color GetColor(string[] colors)
        {
            if(colors.Length == 0) return Color.LightGrey;
            return colors.Length > 1 ? Discord.Color.Gold : new Color(magicColors[colors[0]]);
        }

    }
}
