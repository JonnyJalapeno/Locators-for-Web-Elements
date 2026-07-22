using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Locators_for_Web_Elements
{
    public interface IElementInteractor
    {
        IWebElement FindElementByLocator(By locator);
        IWebElement FindPresentElementByLocator(By locator);
        IEnumerable<IWebElement> FindElementsByLocator(By locator, IWebElement scope);
        IEnumerable<IWebElement> FindElementsByLocator(By locator);
        void ClickElement(By locator);
        void TypeIntoElement(By locator, string text);
        void WaitForUrlToContain(string phrase);
        bool ElementContainsPhrase(IWebElement element, string phrase);
        void AcceptCookies();
    }
}
