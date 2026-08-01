using OpenQA.Selenium;

namespace Locators_for_Web_Elements.Core
{
    public interface IElementInteractor
    {
        IWebElement FindElementByLocator(By locator);
        IWebElement FindPresentElementByLocator(By locator);
        IEnumerable<IWebElement> FindElementsByLocator(By locator, IWebElement scope);
        IEnumerable<IWebElement> FindElementsByLocator(By locator);
        List<IWebElement> FindContainerAndReturnItsElements(By containerLocator, By elementLocator);
        void ClickElement(By locator);
        void ClickElementSafely(By locator);
        void TypeIntoElement(By locator, string text);
        void WaitForUrlToContain(string phrase);
        bool ElementContainsPhrase(IWebElement element, string phrase);
        bool IsElementOnTop(IWebDriver driver, IWebElement element);
        void AcceptCookies(By acceptButtonLocator, By privacyDialogLocator);
        void WaitForPreloaderToDisappear(By preloaderLocator);
    }
}
