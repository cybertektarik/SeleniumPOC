using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class DocumentsTab : BasePage
    {
        private PageControl stcPageCount = new PageControl(By.XPath("//span[contains(@class,'page-description')]"));
        private PageControl btnPageCurrent = new PageControl(By.XPath("//button[contains(@class,'page-button font-weight-bold page-active')]"));
        private PageControl btnPreviousPage = new PageControl(By.XPath("//button[@class='ms-arrow'][@data-icon='chevron-left']/.."), "Previous Page");
        private PageControl btnNextPage = new PageControl(By.XPath("//button[@class='ms-arrow'][@data-icon='chevron-right']/.."), "Next Page");
        private PageControl btnPageNumber(string pageNumber) => new PageControl(By.XPath("//div[contains(@class,'pagination-container')]//button[text()='" + pageNumber + "']"), "Page " + pageNumber);

        public DocumentsTab(IWebDriver driver) : base(driver)
        {
        }

        public string GetPageCountText()
        {
            return stcPageCount.GetText();
        }

        public string GetCurrentPage()
        {
            return btnPageCurrent.GetText();
        }

        public void PreviousPage()
        {
            ClickAndWaitForSpinners(btnPreviousPage);
        }

        public void NextPage()
        {
            ClickAndWaitForSpinners(btnNextPage);
        }

        public bool IsPreviousPageEnabled()
        {
            return btnPreviousPage.IsEnabled;
        }

        public bool IsNextPageEnabled()
        {
            return btnNextPage.IsEnabled;
        }

        public void GoToPage(string pageNumber)
        {
            ClickAndWaitForSpinners(btnPageNumber(pageNumber));
        }
    }
}

