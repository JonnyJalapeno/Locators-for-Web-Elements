using Locators_for_Web_Elements.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements.Business
{
    // Represents a service-category landing page reached from the "Services"
    // navigation dropdown (e.g. /services/artificial-intelligence/generative-ai
    // or /services/artificial-intelligence/responsible-ai).
    public class ServicesPage
    {
        private readonly IWebDriver Driver;
        private readonly IElementInteractor Interactor;
        private readonly WebDriverWait Wait;

        private readonly By RelatedExpertiseSection =
            By.XPath("//*[contains(normalize-space(text()), 'Our Related Expertise')]");

        public ServicesPage(IWebDriver driver, IElementInteractor interactor, WebDriverWait wait)
        {
            Driver = driver;
            Interactor = interactor;
            Wait = wait;
        }

        public string GetTitle()
        {
            Wait.Until(d => !string.IsNullOrWhiteSpace(d.Title));
            return Driver.Title;
        }

        public bool IsRelatedExpertiseSectionDisplayed()
        {
            var section = Interactor.FindPresentElementByLocator(RelatedExpertiseSection);
            return section.Displayed;
        }
    }
}
