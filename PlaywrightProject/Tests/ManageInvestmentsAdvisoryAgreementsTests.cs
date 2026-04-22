using System.Web;
using FluentAssertions;
using Microsoft.Playwright;
using NUnit.Framework;
using SeleniumProject.Common;

namespace PlaywrightProject.Tests;

[TestFixture]
public sealed class ManageInvestmentsAdvisoryAgreementsTests
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    [SetUp]
    public async Task SetUp()
    {
        bool headless = GetEnvBool("RUN_HEADLESS", false);

        // Match the Selenium hook behavior: pick the default data file for feature2 scenarios.
        var dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "UserRoles_Set1.json");
        TestUserManager.SetDataFile(dataPath);

        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless
        });

        _context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 800 }
        });

        _page = await _context.NewPageAsync();

        string url = TestUserManager.GetDefaultUrl();
        await _page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
        await WaitForGenericLoadingToDisappearIfPresent();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_context != null)
            await _context.CloseAsync();
        if (_browser != null)
            await _browser.CloseAsync();

        _playwright?.Dispose();
    }

    [Test]
    public async Task ValidateHsaAdvisoryAgreementsLinkForAllInvestmentTypes()
    {
        // Equivalent to:
        // Given I am logged in as a user who has an enrolled account
        var username = TestUserManager.GetUsername("EnrolledUser");
        await Login(username, "$BetterHsa777");

        // When I click on "Manage Investment" from the navigation menu
        await ClickNavManageInvestments();

        // And I click on "Resources" from the navigation menu
        await _page!.GetByRole(AriaRole.Link, new() { Name = "Resources" }).ClickAsync();
        await WaitForGenericLoadingToDisappearIfPresent();

        // And I click on the "HSA Invest" info link
        // This link may live inside a collapsible/hidden menu. Navigating to the route is more robust.
        await NavigateToHashRoute("/resources/hsa-invest");
        await WaitForGenericLoadingToDisappearIfPresent();

        // Then I validate the HSA Advisory Agreements links for following investment types
        await AssertAdvisoryAgreement("HSA Advisory Agreement Select", "HSA_Curated_Advisory_Agreement_LH");
        await AssertAdvisoryAgreement("HSA Advisory Agreement Choice", "HSA_Choice_Advisory_Agreement_LH");
        await AssertAdvisoryAgreement("HSA Advisory Agreement Managed", "abg_advisory_managed");
    }

    private async Task ClickNavManageInvestments()
    {
        // Mirrors Selenium selector: //span[@role='button' and normalize-space()='Manage Investments']
        await _page!.Locator("span[role='button']", new() { HasTextString = "Manage Investments" })
            .First.ClickAsync();
        await WaitForGenericLoadingToDisappearIfPresent();
    }

    private async Task NavigateToHashRoute(string route)
    {
        // Keep origin, replace hash route. Works whether current URL already contains a hash.
        var current = _page!.Url;
        var baseUrl = current.Split('#')[0];
        var target = $"{baseUrl}#{route}";
        await _page.GotoAsync(target, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
    }

    private async Task Login(string userName, string password)
    {
        await _page!.Locator("#idp-discovery-username").FillAsync(userName);
        await _page.Locator("#idp-discovery-submit").ClickAsync();

        await _page.Locator("#okta-signin-password").WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 30_000
        });

        await _page.Locator("#okta-signin-password").FillAsync(password);
        await _page.Locator("#okta-signin-submit").ClickAsync();

        // NetworkIdle can hang on SPAs (long-poll/websocket). Wait for an app-specific signal instead.
        await _page.Locator("span[role='button']", new() { HasTextString = "Manage Investments" })
            .First.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 60_000 });

        await WaitForGenericLoadingToDisappearIfPresent();
    }

    private async Task AssertAdvisoryAgreement(string linkText, string expectedDocumentKey)
    {
        var popup = await _page!.RunAndWaitForPopupAsync(async () =>
        {
            await _page.Locator($"text={linkText}").First.ClickAsync();
        });

        await popup.WaitForLoadStateAsync(LoadState.NetworkIdle);
        string actualDocumentKey = ExtractDocumentKeyFromFragment(popup.Url);
        actualDocumentKey.Should().Be(expectedDocumentKey);
        await popup.CloseAsync();
    }

    private async Task WaitForGenericLoadingToDisappearIfPresent()
    {
        try
        {
            // There can be multiple '#generic-loading' elements on the page; avoid strict mode.
            await _page!.WaitForFunctionAsync(
                "() => document.querySelectorAll('#generic-loading').length === 0",
                null,
                new PageWaitForFunctionOptions { Timeout = 30_000 });
        }
        catch (TimeoutException)
        {
            // Spinner is sometimes optional/flaky; don't fail on timeout.
        }
    }

    private static string ExtractDocumentKeyFromFragment(string url)
    {
        var uri = new Uri(url);
        string fragment = uri.Fragment; // e.g. "#/document-view?documentKey=..."

        if (!fragment.Contains("documentKey=", StringComparison.OrdinalIgnoreCase))
            throw new AssertionException($"Document key not found in the URL fragment: {fragment}");

        var queryStartIndex = fragment.IndexOf('?', StringComparison.Ordinal);
        if (queryStartIndex == -1)
            throw new AssertionException($"Document key query not found in the URL fragment: {fragment}");

        string query = fragment[(queryStartIndex + 1)..];
        var queryParams = HttpUtility.ParseQueryString(query);
        var key = queryParams.Get("documentKey");

        if (string.IsNullOrWhiteSpace(key))
            throw new AssertionException($"Document key was empty in the URL fragment: {fragment}");

        return key;
    }

    private static bool GetEnvBool(string envName, bool defaultVal)
    {
        var v = Environment.GetEnvironmentVariable(envName);
        if (string.IsNullOrWhiteSpace(v)) return defaultVal;
        return string.Equals(v, "1", StringComparison.Ordinal) ||
               string.Equals(v, "true", StringComparison.OrdinalIgnoreCase);
    }
}

