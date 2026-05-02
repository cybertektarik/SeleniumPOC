using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightProject.Tests.Shared;
using PlaywrightProject.Web;

namespace PlaywrightProject.Tests.Web;

[TestFixture]
public sealed class ManageInvestmentsAdvisoryAgreementsWebTests : ManageInvestmentsAdvisoryAgreementsTestBase
{
    protected override BrowserNewContextOptions GetContextOptions(IPlaywright playwright) =>
        WebBrowserContextOptions.CreateDesktop();
}
