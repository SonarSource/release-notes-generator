using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReleaseNotes.GitHub;

namespace ReleaseNotes.Helpers
{
    public class ReleaseNotesFormatter
    {
        private readonly TextWriter writer;

        public ReleaseNotesFormatter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Format(IEnumerable<Issue> issues)
        {
            var issuesByKind = issues.ToLookup(IssueTypeHelper.GetIssueType, i => i);

            PrintGroup("### New Rules", issuesByKind[IssueType.NewRule]);
            PrintGroup("### Improvements", issuesByKind[IssueType.Improvement]);
            PrintGroup("### Bug Fixes", issuesByKind[IssueType.BugFix]);
            PrintGroup("### False Positive", issuesByKind[IssueType.FalsePositive]);
            PrintGroup("### False Negative", issuesByKind[IssueType.FalseNegative]);
            // Do not print issues from the IssueKind.Hidden group
        }

        private void PrintGroup(string groupTitle, IEnumerable<Issue> issues)
        {
            // Skip empty groups
            if (!issues.Any())
            {
                return;
            }

            writer.WriteLine(groupTitle);

            foreach (var issue in issues)
            {
                writer.WriteLine(ToBulletItem(issue));
            }
        }

        private string ToBulletItem(Issue issue) =>
            $"* [{issue.Number}]({issue.HtmlUrl}) - {issue.Title}";
    }
}
