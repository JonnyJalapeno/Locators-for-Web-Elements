using Locators_for_Web_Elements.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements.Business
{
    public class HomeCarousel
    {
        private readonly IWebDriver Driver;
        private readonly WebDriverWait Wait;
        private readonly IPageFactory PageFactory;

        private readonly By _root = By.XPath("//div[contains(@class,'slider') and contains(@class,'section')]");
        private readonly By _activeSlide = By.XPath(".//div[contains(@class,'owl-item') and contains(@class,'active')]//div[contains(@class,'single-slide-ui')]");
        private readonly By _titleParagraph = By.XPath(".//div[contains(@class, 'single-slide__content-container')]");
        private readonly By _readMoreLink = By.XPath(".//a[contains(@class,'slider-cta-link')]");

        public HomeCarousel(IWebDriver driver, WebDriverWait wait, IPageFactory pageFactory)
        {
            Driver = driver;
            Wait = wait;
            PageFactory = pageFactory;
        }

        private IWebElement Root => Wait.Until(d => d.FindElement(_root));

        public HomeCarousel Swipe(int times = 1)
        {
            const int stepOffset = -25;
            const int stepCount = 6;

            for (int i = 0; i < times; i++)
            {
                var titleBefore = GetActiveSlideTitle();
                var slideArea = Root.FindElement(_activeSlide);

                var actions = new Actions(Driver)
                    .MoveToElement(slideArea)
                    .ClickAndHold();

                for (int s = 0; s < stepCount; s++)
                {
                    actions = actions.MoveByOffset(stepOffset, 0).Pause(TimeSpan.FromMilliseconds(80));
                }

                actions
                    .Release()
                    .Pause(TimeSpan.FromMilliseconds(500))
                    .Build()
                    .Perform();

                Wait.Until(d => GetActiveSlideTitle() != titleBefore);
            }
            return this;
        }

        public string GetActiveSlideTitle()
        {
            var titleEl = Root.FindElement(_activeSlide).FindElement(_titleParagraph);
            return NormalizeText(titleEl.Text);
        }

        public ArticlePage ClickReadMoreOnActiveSlide()
        {
            var link = Root.FindElement(_activeSlide).FindElement(_readMoreLink);
            Wait.Until(_driver => { return link.Enabled ? link : null; }).Click();
            return PageFactory.Create<ArticlePage>();
        }

        public static string NormalizeText(string text) =>
            string.Join(" ", text.Split(new[] { ' ', '\u00A0', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries));
    }
}
