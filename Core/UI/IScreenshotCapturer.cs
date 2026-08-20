using OpenQA.Selenium;

namespace Locators_for_Web_Elements.Core.UI
{
    public interface IScreenshotCapturer
    {
        string Capture(IWebDriver driver, string testName);
    }
}
