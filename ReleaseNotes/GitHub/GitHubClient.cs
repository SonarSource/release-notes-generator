using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReleaseNotes.GitHub
{
    public class GitHubClient
    {
        private readonly HttpClient httpClient;

        public GitHubClient(HttpClient httpClient)
        {
            var baseAddress = httpClient.BaseAddress.ToString();
            // The requests we execute require the BaseAddress to end with /
            if (!baseAddress.EndsWith('/'))
            {
                throw new System.ArgumentOutOfRangeException(nameof(httpClient),
                    "HttpClient.BaseAddress should end with '/'");
            }
            // The BaseAddress should be contain the repository name
            if (!baseAddress.StartsWith("https://api.github.com/repos/") ||
                baseAddress.Length <= "https://api.github.com/repos/".Length)
            {
                throw new System.ArgumentOutOfRangeException(nameof(httpClient),
                    "HttpClient.BaseAddress should start with 'https://api.github.com/repos' and contain repository name");
            }

            this.httpClient = httpClient;
        }

        public async Task<Milestone[]> GetMilestonesAsync()
        {
            var milestonesString = await httpClient.GetStringAsync(
                // For some reason GitHub does not return all milestones, hence we
                // sort by title in descending order to obtain the newest versions
                $"milestones?state=all&direction=desc&sort=title");

            return JsonConvert.DeserializeObject<Milestone[]>(milestonesString);
        }

        public async Task<Issue[]> GetIssuesAsync(int milestoneNumber)
        {
            var issuesString = await httpClient.GetStringAsync(
                $"issues?state=all&milestone={milestoneNumber}");

            return JsonConvert.DeserializeObject<Issue[]>(issuesString);
        }
    }
}
