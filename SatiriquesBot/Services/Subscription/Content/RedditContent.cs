using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SatiriquesBot.Services.Subscription.Content
{
    public class RedditContent : ContentBase
    {
        [JsonProperty("?xml")] public Xml Xml { get; set; }
        public Feed feed { get; set; }
    }

    
    
    // generated code
    
    public class Xml
    {
        [JsonProperty("@version")] public string Version { get; set; }

        [JsonProperty("@encoding")] public string Encoding { get; set; }
    }

    public class Category
    {
        [JsonProperty("@term")] public string Term { get; set; }

        [JsonProperty("@label")] public string Label { get; set; }
    }

    public class Link
    {
        [JsonProperty("@rel")] public string Rel { get; set; }

        [JsonProperty("@href")] public string Href { get; set; }

        [JsonProperty("@type")] public string Type { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
        public string uri { get; set; }
    }

    public class Content
    {
        [JsonProperty("@type")] public string Type { get; set; }

        [JsonProperty("#text")] public string Text { get; set; }
    }

    public class MediaThumbnail
    {
        [JsonProperty("@url")] public string Url { get; set; }
    }

    public class Entry
    {
        public Author author { get; set; }
        public Category category { get; set; }
        public Content content { get; set; }
        public string id { get; set; }
        public Link link { get; set; }
        public DateTime updated { get; set; }
        public DateTime published { get; set; }
        public string title { get; set; }

        [JsonProperty("media:thumbnail")] public MediaThumbnail MediaThumbnail { get; set; }
    }

    public class Feed
    {
        [JsonProperty("@xmlns")] public string Xmlns { get; set; }

        [JsonProperty("@xmlns:media")] public string XmlnsMedia { get; set; }
        public Category category { get; set; }
        public DateTime updated { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public List<Link> link { get; set; }
        public string logo { get; set; }
        public string subtitle { get; set; }
        public string title { get; set; }
        public List<Entry> entry { get; set; }
    }
    

    
}