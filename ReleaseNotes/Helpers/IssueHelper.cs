using ReleaseNotes.GitHub;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReleaseNotes.Helpers
{
    public static class IssueHelper
    {
        private static readonly Dictionary<string, string> areasMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Area: C#", "C#" },
            { "Area: VB.NET", "VB.NET" },
        };

        private static readonly Dictionary<string, IssueType> issueTypesMap = new Dictionary<string, IssueType>(StringComparer.OrdinalIgnoreCase)
        {
            { "Type: Improvement", IssueType.Improvement },
            { "Type: New Feature", IssueType.NewRule },
            { "Type: Bug", IssueType.BugFix },
            { "Type: False Positive", IssueType.FalsePositive },
            { "Type: False Negative", IssueType.FalseNegative },
            { "Type: Performance", IssueType.Performance },
            { "Type: Task", IssueType.Hidden }
        };

        public static IssueType GetIssueType(this Issue issue)
        {
            var typeLabel = issue.Labels
                .Select(label => label.Name)
                .FirstOrDefault(issueTypesMap.ContainsKey);

            return issueTypesMap[typeLabel ?? "Type: Improvement"];
        }

        public static string ToBulletItem(this Issue issue) =>
            $"* [{issue.Number}]({issue.HtmlUrl}) - {issue.GetAreaPrefix()}{issue.Title}";

        public static string GetAreaPrefix(this Issue issue)
        {
            var areas = issue.Labels
                .Select(label => areasMap.GetValueOrDefault(label.Name))
                .Where(area => area != null)
                .OrderBy(area => area)
                .ToList();

            return areas.Count == 0
                ? string.Empty
                : $"[{string.Join(", ", areas)}] ";
        }
    }
}
