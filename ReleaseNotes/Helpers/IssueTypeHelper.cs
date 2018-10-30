using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseNotes.GitHub;

namespace ReleaseNotes.Helpers
{
    public static class IssueTypeHelper
    {
        private static readonly Dictionary<string, IssueType> map = new Dictionary<string, IssueType>(StringComparer.OrdinalIgnoreCase)
        {
            { "Type: Improvement", IssueType.Improvement },
            { "Type: New Feature", IssueType.NewRule },
            { "Type: Bug", IssueType.BugFix },
            { "Type: False Positive", IssueType.FalsePositive },
            { "Type: False Negative", IssueType.FalseNegative },
            { "Type: Task", IssueType.Hidden },
        };

        public static IssueType GetIssueType(Issue issue)
        {
            var typeLabel = issue.Labels
                .Select(label => label.Name)
                .FirstOrDefault(map.ContainsKey);

            return map[typeLabel ?? "Type: Improvement"];
        }
    }
}
