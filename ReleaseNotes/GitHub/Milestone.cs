using System.Diagnostics;
using Newtonsoft.Json;

namespace ReleaseNotes.GitHub
{
    [DebuggerDisplay("{Title} - {Number}")]
    public class Milestone
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("number")]
        public int Number { get; set; }
    }
}
