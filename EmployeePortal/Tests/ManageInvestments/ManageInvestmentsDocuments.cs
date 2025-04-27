using FluentAssertions;
using NUnit.Framework;

namespace SeleniumPOC.EmployeePortal.Tests.ManageInvestments
{
    [Parallelizable(ParallelScope.Self)]
    public class ManageInvestmentsDocuments : EmployeePortalBaseTestSuite
    {
        [Test]
        public void VerifyInvestmentDocumentsPageControls()
        {
            TestContext.WriteLine("Login and go to the Manage Investments tab");
            Pages.LoginPage.Login("bendlocaltest+payton34@gmail.com");
            Pages.SidebarNavPage.GoToManageInvestments();

            Console.WriteLine("Go to the Manage Investments -> Documents tab");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseChoice();
            Pages.ManageInvestmentsPage.ClickDocumentsTab();
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Documents");
            Pages.ManageInvestmentsPage.DocumentsTab.GetPageCountText().Should().Be("PAGE 1 OF 4");

            Console.WriteLine("Go through all the Document pages using the Next arrow");
            Pages.ManageInvestmentsPage.DocumentsTab.IsPreviousPageEnabled().Should().BeFalse();
            Pages.ManageInvestmentsPage.DocumentsTab.NextPage();
            Pages.ManageInvestmentsPage.DocumentsTab.GetCurrentPage().Should().Be("2");
            Pages.ManageInvestmentsPage.DocumentsTab.GetPageCountText().Should().Be("PAGE 2 OF 4");

            Pages.ManageInvestmentsPage.DocumentsTab.NextPage();
            Pages.ManageInvestmentsPage.DocumentsTab.GetCurrentPage().Should().Be("3");
            Pages.ManageInvestmentsPage.DocumentsTab.GetPageCountText().Should().Be("PAGE 3 OF 4");

            Pages.ManageInvestmentsPage.DocumentsTab.NextPage();
            Pages.ManageInvestmentsPage.DocumentsTab.GetCurrentPage().Should().Be("4");
            Pages.ManageInvestmentsPage.DocumentsTab.GetPageCountText().Should().Be("PAGE 4 OF 4");
            Pages.ManageInvestmentsPage.DocumentsTab.IsNextPageEnabled().Should().BeFalse();

            Console.WriteLine("Go through all the Document pages using the Previous arrow");
            Pages.ManageInvestmentsPage.DocumentsTab.PreviousPage();
            Pages.ManageInvestmentsPage.DocumentsTab.GetCurrentPage().Should().Be("3");
            Pages.ManageInvestmentsPage.DocumentsTab.GetPageCountText().Should().Be("PAGE 3 OF 4");
            Pages.ManageInvestmentsPage.DocumentsTab.IsNextPageEnabled().Should().BeTrue();

            Pages.ManageInvestmentsPage.DocumentsTab.PreviousPage();
            Pages.ManageInvestmentsPage.DocumentsTab.GetCurrentPage().Should().Be("2");
            Pages.ManageInvestmentsPage.DocumentsTab.GetPageCountText().Should().Be("PAGE 2 OF 4");

            Pages.ManageInvestmentsPage.DocumentsTab.PreviousPage();
            Pages.ManageInvestmentsPage.DocumentsTab.GetCurrentPage().Should().Be("1");
            Pages.ManageInvestmentsPage.DocumentsTab.GetPageCountText().Should().Be("PAGE 1 OF 4");
            Pages.ManageInvestmentsPage.DocumentsTab.IsPreviousPageEnabled().Should().BeFalse();

            Console.WriteLine("Use specific page number");
            Pages.ManageInvestmentsPage.DocumentsTab.GoToPage("3");
            Pages.ManageInvestmentsPage.DocumentsTab.GetCurrentPage().Should().Be("3");
            Pages.ManageInvestmentsPage.DocumentsTab.GetPageCountText().Should().Be("PAGE 3 OF 4");
        }
    }
}

