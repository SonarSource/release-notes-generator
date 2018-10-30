using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseNotes.GitHub;
using ReleaseNotes.Helpers;

namespace release_notes_tests
{
    [TestClass]
    public class IssueTypeHelper_GetIssueType
    {
        [TestMethod]
        public void GetIssueType_Existing_Labels_Improvement()
        {
            var improvement = new Issue { Labels = new[] { new Label { Name = "Type: Improvement" } } };
            IssueTypeHelper.GetIssueType(improvement).Should().Be(IssueType.Improvement);
        }

        [TestMethod]
        public void GetIssueType_Existing_Labels_NewRule()
        {
            var newFeature = new Issue { Labels = new[] { new Label { Name = "Type: New Feature" } } };
            IssueTypeHelper.GetIssueType(newFeature).Should().Be(IssueType.NewRule);
        }

        [TestMethod]
        public void GetIssueType_Existing_Labels_BugFix()
        {
            var bug = new Issue { Labels = new[] { new Label { Name = "Type: Bug" } } };
            IssueTypeHelper.GetIssueType(bug).Should().Be(IssueType.BugFix);
        }

        [TestMethod]
        public void GetIssueType_Existing_Labels_FalsePositive()
        {
            var falsePositive = new Issue { Labels = new[] { new Label { Name = "Type: False Positive" } } };
            IssueTypeHelper.GetIssueType(falsePositive).Should().Be(IssueType.FalsePositive);
        }

        [TestMethod]
        public void GetIssueType_Existing_Labels_FalseNegative()
        {
            var falseNegative = new Issue { Labels = new[] { new Label { Name = "Type: False Negative" } } };
            IssueTypeHelper.GetIssueType(falseNegative).Should().Be(IssueType.FalseNegative);
        }

        [TestMethod]
        public void GetIssueType_Existing_Labels_Task()
        {
            var task = new Issue { Labels = new[] { new Label { Name = "Type: Task" } } };
            IssueTypeHelper.GetIssueType(task).Should().Be(IssueType.Hidden);
        }

        [TestMethod]
        public void GetIssueType_Case_Insensitive()
        {
            var task = new Issue { Labels = new[] { new Label { Name = "type: task" } } };
            IssueTypeHelper.GetIssueType(task).Should().Be(IssueType.Hidden);
        }

        [TestMethod]
        public void GetIssueType_No_Labels()
        {
            var noLabel = new Issue { Labels = new Label[0] };
            IssueTypeHelper.GetIssueType(noLabel).Should().Be(IssueType.Improvement);
        }

        [TestMethod]
        public void GetIssueType_Multiple_Labels()
        {
            var task = new Issue
            {
                Labels = new[]
                {
                    new Label { Name = "Type: False Positive" },
                    new Label { Name = "Type: Bug" }
                }
            };
            // The first label determines the type
            IssueTypeHelper.GetIssueType(task).Should().Be(IssueType.FalsePositive);
        }
    }
}
