using OpenQA.Selenium;
//using OpenQA.Selenium.Support.UI;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class WizardRtqQuestionsPage : BasePage
    {
        public WizardRtqQuestionsPage(IWebDriver driver) : base(driver)
        {
        }

        private PageControl radioQuestion(string num, string value) =>
            new PageControl(By.XPath($"(//div[@data-vv-name='question " + num + " answer']//input[@value='" + value + "'])[last()]"));

        private PageControl btnSubmit = new PageControl(By.XPath("//button[text()='Submit']"));

        public void SetAnswerForQuestion(string question, string value)
        {
            Thread.Sleep(4000);
            // wait for element to be visible
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//div[@data-vv-name='question " + question + " answer']//input[@value='" + value + "'])[last()]")));
            //ElementIsVisible(By.XPath("(//div[@data-vv-name='question " + question + " answer']//input[@value='" + value + "']/..)[2]")));



            // Get PageControl's Object
            var element = radioQuestion(question, value);
            element.SendKeysUsingActions(Keys.Up);

            //try
            //{
            //    // With JavaScript scroll
            //    string script = "var element = document.evaluate(\"//div[@data-vv-name='question " + question + " answer']//input[@value='" + value + "']\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; if(element) { element.scrollIntoView({block: 'center'}); }";
            //    ((IJavaScriptExecutor)driver).ExecuteScript(script);

            //    Thread.Sleep(500); // wait for scroll to complete

            //    // try regular click
            //    element.Click();
            //}
            //catch (Exception)
            //{
            //    // if it is not successful, try regular click again
            //    element.Click();
            //}
        }

        public void Submit()
        {
            btnSubmit.Click();
            WaitForSpinners();
        }
    }
}

