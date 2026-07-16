using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements
{
    public class InsightsPage : BaseComponent<InsightsPage>
    {
        public HomeCarousel Carousel { get; }

        public InsightsPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
            Carousel = new HomeCarousel(driver, wait);
        }
    }
}
