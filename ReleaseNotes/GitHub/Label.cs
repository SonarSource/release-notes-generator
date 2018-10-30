using System.Diagnostics;
using Newtonsoft.Json;

namespace ReleaseNotes.GitHub
{
    [DebuggerDisplay("{Name}")]
    public class Label
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
