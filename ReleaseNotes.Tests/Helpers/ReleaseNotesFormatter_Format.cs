using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text;

namespace ReleaseNotes.Helpers
{
    [TestClass]
    public class ReleaseNotesFormatter_Format
    {
        private StringBuilder sb;
        private ReleaseNotesFormatter formatter;

        [TestInitialize]
        public void TestInitialize()
        {
            sb = new StringBuilder();
            formatter = new ReleaseNotesFormatter(new StringWriter(sb));
        }

        [TestMethod]
        public void Format_Does_Not_Print_Tasks_And_Empty_Groups()
        {
            var issues = new[]
            {
                GetIssue(1, "Type: Task"),
                GetIssue(2, "Type: Task"),
            };

            formatter.Format(issues);

            sb.ToString().Should().Be(@"");
        }

        [TestMethod]
        public void Format_Prints_Issues_In_Ordered_Groups()
        {
            var issues = new[]
            {
                GetIssue(1, "Type: Improvement"),
                GetIssue(2, "Type: New Feature"),
                GetIssue(3, "Type: Bug"),
                GetIssue(4, "Type: False Positive"),
                GetIssue(5, "Type: False Negative"),
                GetIssue(6, "Type: Task"),
            };

            formatter.Format(issues);

            sb.ToString().Should().Be(@"### New Rules
* [2](https://url/path/2) - title 2
### Improvements
* [1](https://url/path/1) - title 1
### Bug Fixes
* [3](https://url/path/3) - title 3
### False Positive
* [4](https://url/path/4) - title 4
### False Negative
* [5](https://url/path/5) - title 5
");
        }

        private static GitHub.Issue GetIssue(int number, params string[] labels) =>
            new GitHub.Issue
            {
                Number = number,
                Title = $"title {number}",
                HtmlUrl = $"https://url/path/{number}",
                Labels = labels.Select(label => new GitHub.Label { Name = label }).ToArray(),
            };
    }
}
