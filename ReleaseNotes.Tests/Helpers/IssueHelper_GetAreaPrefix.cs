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

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseNotes.Helpers
{
    [TestClass]
    public class IssueHelper_GetAreaPrefix
    {
        [TestMethod]
        public void GetAreaPrefix_No_Label()
        {
            IssueHelper.GetAreaPrefix(new GitHub.Issue { Labels = new GitHub.Label[0] }).Should().BeEmpty();
        }

        [TestMethod]
        public void GetAreaPrefix_VB()
        {
            var labels = new[]
            {
                new GitHub.Label { Name = "Area: VB.NET" }
            };
            IssueHelper.GetAreaPrefix(new GitHub.Issue { Labels = labels }).Should().Be("[VB.NET] ");
        }

        [TestMethod]
        public void GetAreaPrefix_CS()
        {
            var labels = new[]
            {
                new GitHub.Label { Name = "Area: C#" }
            };
            IssueHelper.GetAreaPrefix(new GitHub.Issue { Labels = labels }).Should().Be("[C#] ");
        }

        [TestMethod]
        public void GetAreaPrefix_VB_and_CS()
        {
            var labels = new[]
            {
                new GitHub.Label { Name = "Area: VB.NET" },
                new GitHub.Label { Name = "Area: C#" },
            };
            IssueHelper.GetAreaPrefix(new GitHub.Issue { Labels = labels }).Should().Be("[C#, VB.NET] ");
        }
    }
}
