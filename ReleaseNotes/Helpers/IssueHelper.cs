/*
 * SonarAnalyzer for .NET
 * Copyright (C) 2015-2020 SonarSource SA
 * mailto: contact AT sonarsource DOT com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

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
