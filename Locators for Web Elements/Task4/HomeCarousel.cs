using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace EpamTests.PageObjects.Components
{
    public class HomeCarousel
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private readonly By _root = By.XPath("//div[contains(@class,'slider') and contains(@class,'section')]");
        private readonly By _activeSlide = By.XPath(".//div[contains(@class,'owl-item') and contains(@class,'active')]//div[contains(@class,'single-slide-ui')]");
        private readonly By _titleParagraph = By.XPath(".//div[contains(@class, 'single-slide__content-container')]");
        private readonly By _readMoreLink = By.XPath(".//a[contains(@class,'slider-cta-link')]");

        public HomeCarousel(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        private IWebElement Root => _wait.Until(d => d.FindElement(_root));

        // Action — drags the active slide horizontally to trigger a swipe
        public HomeCarousel Swipe(int times = 1)
        {
            const int stepOffset = -25;   // per-step distance (negative = drag left = next slide)
            const int stepCount = 4;      // ~100px total, in 4 discrete moves

            for (int i = 0; i < times; i++)
            {
                var titleBefore = GetActiveSlideTitle();
                var slideArea = Root.FindElement(_activeSlide);

                var actions = new Actions(_driver).MoveToElement(slideArea).ClickAndHold();
                for (int s = 0; s < stepCount; s++)
                {
                    actions = actions.MoveByOffset(stepOffset, 0).Pause(TimeSpan.FromMilliseconds(50));
                }
                actions.Release().Build().Perform();

                _wait.Until(d => GetActiveSlideTitle() != titleBefore);
                WaitForCarouselToSettle();
            }
            return this;
        }

        private void WaitForCarouselToSettle()
        {
            _wait.Until(d =>
            {
                var stage = Root.FindElement(By.CssSelector(".owl-stage"));
                var styleBefore = stage.GetAttribute("style");
                System.Threading.Thread.Sleep(150);
                var styleAfter = stage.GetAttribute("style");
                return styleBefore == styleAfter;
            });
        }

        // Query
        public string GetActiveSlideTitle()
        {
            var titleEl = Root.FindElement(_activeSlide).FindElement(_titleParagraph);
            return NormalizeText(titleEl.Text);
        }

        // Action
        public void ClickReadMoreOnActiveSlide()
        {
            var link = Root.FindElement(_activeSlide).FindElement(_readMoreLink);
            _wait.Until(_driver=> { return link.Enabled ? link : null; }).Click();
        }

        public static string NormalizeText(string text) =>
            string.Join(" ", text.Split(new[] { ' ', '\u00A0', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries));
    }
}