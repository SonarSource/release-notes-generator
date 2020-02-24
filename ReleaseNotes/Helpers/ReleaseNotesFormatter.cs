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
            var issuesByKind = issues.ToLookup(IssueHelper.GetIssueType, i => i);

            PrintGroup("### New Rules", issuesByKind[IssueType.NewRule]);
            PrintGroup("### Improvements", issuesByKind[IssueType.Improvement]);
            PrintGroup("### Bug Fixes", issuesByKind[IssueType.BugFix]);
            PrintGroup("### False Positive", issuesByKind[IssueType.FalsePositive]);
            PrintGroup("### False Negative", issuesByKind[IssueType.FalseNegative]);
            PrintGroup("### Performance", issuesByKind[IssueType.Performance]);
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
                writer.WriteLine(issue.ToBulletItem());
            }
        }
    }
}
