using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReleaseNotes.GitHub
{
    public class GitHubClient
    {
        private const string LinkHeaderIdentifier = "Link";
        private const string NextLinkHeaderIdentifier = "NEXT";
        
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
            // The BaseAddress should contain the repository name
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
            var url = $"issues?state=all&milestone={milestoneNumber}";

            return await GetIssuesAsync(url);
        }

        private async Task<Issue[]> GetIssuesAsync(string url)
        {
            var response = await httpClient.GetAsync(url);

            var issuesString = await response.Content.ReadAsStringAsync();
            var issues = JsonConvert.DeserializeObject<Issue[]>(issuesString);
            var nextPageUrl = GetNextPageUrl(response.Headers);

            return string.IsNullOrEmpty(nextPageUrl)
                ? issues
                : issues.Concat(await GetIssuesAsync(nextPageUrl)).ToArray();
        } 

        private static string GetNextPageUrl(HttpHeaders headers)
        {
            var linkHeader = headers.GetValues(LinkHeaderIdentifier).FirstOrDefault();
            if (linkHeader == null)
            {
                return null;
            }
            
            var nextPageUrlQuery = from link in linkHeader.Split(',')
                                   let relMatch = Regex.Match(link, "(?<=rel=\").+?(?=\")", RegexOptions.IgnoreCase)
                                   where relMatch.Success && relMatch.Value.ToUpper() == NextLinkHeaderIdentifier
                                   let linkMatch = Regex.Match(link, "(?<=<).+?(?=>)", RegexOptions.IgnoreCase)
                                   where linkMatch.Success
                                   select linkMatch.Value;
            
            return nextPageUrlQuery.FirstOrDefault();
        }
    }
}
