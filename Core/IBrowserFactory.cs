using OpenQA.Selenium;

namespace Locators_for_Web_Elements.Core
{
    public interface IBrowserFactory
    {
        IWebDriver Create();
    }
}
