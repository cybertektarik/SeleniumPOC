using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightProject.Mobile;
using PlaywrightProject.Tests.Shared;

namespace PlaywrightProject.Tests.Mobile;

[TestFixture]
public sealed class ManageInvestmentsAdvisoryAgreementsMobileTests : ManageInvestmentsAdvisoryAgreementsTestBase
{
    protected override BrowserNewContextOptions GetContextOptions(IPlaywright playwright) =>
        MobileBrowserContextOptions.CreateForIos(playwright);
}
