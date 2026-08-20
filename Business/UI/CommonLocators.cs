using OpenQA.Selenium;

namespace Locators_for_Web_Elements.Business.UI
{
    // Shared, site-wide locators used by more than one page object (e.g. the cookie
    // consent banner, which can appear on both HomePage and CareerPage).
    internal static class CommonLocators
    {
        public static readonly By CookiesAcceptButton = By.Id("onetrust-accept-btn-handler");
        public static readonly By PrivacyDialog = By.CssSelector("div[role='dialog'][aria-label='Privacy']");
    }
}
