using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ReleaseNotes.GitHub;
using ReleaseNotes.Helpers;

namespace ReleaseNotes
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var repo = args.ElementAtOrDefault(0);
            if (repo == null)
            {
                Console.WriteLine("Please specify repository name as first argument, including owner.");
                return;
            }

            var milestoneName = args.ElementAtOrDefault(1);
            if (milestoneName == null)
            {
                Console.WriteLine("Please specify milestone name as second argument.");
                return;
            }

            var token = args.ElementAtOrDefault(2);
            if (milestoneName == null)
            {
                Console.WriteLine("Please specify GitHub access token as third argument.");
                return;
            }

            var gitHubClient = CreateGitHubClient(repo, token);

            PrintReleaseNotes(gitHubClient, milestoneName)
                .GetAwaiter()
                .GetResult();
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

        public static async Task PrintReleaseNotes(GitHubClient gitHubClient, string milestoneName)
        {
            var milestones = await gitHubClient.GetMilestonesAsync();

            var milestone = milestones.FirstOrDefault(m => m.Title == milestoneName);
            if (milestone == null)
            {
                Console.WriteLine($"Cannot find milestone with name '{milestoneName}'");
                return;
            }

            var issues = await gitHubClient.GetIssuesAsync(milestone.Number);

            var formatter = new ReleaseNotesFormatter(Console.Out);

            formatter.Format(issues);
        }
    }
}
