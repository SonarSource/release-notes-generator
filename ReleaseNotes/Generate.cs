using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ReleaseNotes.GitHub;
using ReleaseNotes.Helpers;

namespace ReleaseNotes
{
    public static class Generate
    {
        private const string FunctionName = "gen";
        private const string RepoArgument = "r";
        private const string MilestoneArgument = "m";
        private const string TokenArgument = "t";

        [FunctionName(FunctionName)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string repo = request.Query[RepoArgument];
            string milestoneName = request.Query[MilestoneArgument];
            string token = request.Query[TokenArgument];

            if (string.IsNullOrWhiteSpace(repo) ||
                string.IsNullOrWhiteSpace(milestoneName) ||
                string.IsNullOrWhiteSpace(token))
            {
                return new BadRequestObjectResult($"Please, provide the following URL parameters: " +
                    $"&{RepoArgument}=repository-name&{MilestoneArgument}=milestone-name&{TokenArgument}=github-token");
            }

            var gitHubClient = CreateGitHubClient(repo, token);

            var milestones = await gitHubClient.GetMilestonesAsync();

            var milestone = milestones.FirstOrDefault(m => m.Title == milestoneName);
            if (milestone == null)
            {
                log.LogError($"Cannot find milestone with name '{milestoneName}'");
                return new BadRequestObjectResult($"Cannot find milestone with name '{milestoneName}'");
            }

            var issues = await gitHubClient.GetIssuesAsync(milestone.Number);

            var sb = new StringBuilder();

            var formatter = new ReleaseNotesFormatter(new StringWriter(sb));

            formatter.Format(issues);

            return new OkObjectResult(sb.ToString());
        }

        private static GitHubClient CreateGitHubClient(string repo, string token)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://api.github.com/repos/{repo}/"),
                DefaultRequestHeaders =
                {
                    { "User-Agent", "release-notes-generator" },
                    { "Authorization", token },
                }
            };

            return new GitHubClient(httpClient);
        }
    }
}
