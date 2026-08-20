using OpenQA.Selenium;

namespace Locators_for_Web_Elements.Core.UI
{
    public interface IBrowserFactory
    {
        IWebDriver Create();
    }
}
