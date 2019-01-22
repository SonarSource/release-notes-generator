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
