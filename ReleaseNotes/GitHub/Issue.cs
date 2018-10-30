using System.Diagnostics;
using Newtonsoft.Json;

namespace ReleaseNotes.GitHub
{
    [DebuggerDisplay("{Title} - {Number}")]
    public class Issue
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("number")]
        public int Number { get; set; }
        [JsonProperty("labels")]
        public Label[] Labels { get; set; }
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }
}
