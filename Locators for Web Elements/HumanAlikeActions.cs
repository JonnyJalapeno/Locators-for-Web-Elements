using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Locators_for_Web_Elements
{
    public static class HumanAlikeActions
    {

        private static readonly Random Random = new Random();

        public static void HumanPause(double minDelay = 0.2, double maxDelay = 1.0)
        {
            int milliseconds = (int)(Random.NextDouble() * (maxDelay - minDelay) * 1000 + minDelay * 1000);
            Thread.Sleep(milliseconds);
        }
        public static void HumanClick(IWebElement element, double minDelay = 0.2, double maxDelay = 1.0)
        {
            HumanPause(minDelay, maxDelay);
            element.Click();
        }

        public static void HumanSendKeys(IWebElement element, string text, double minDelay = 0.2, double maxDelay = 1.0)
        {
            HumanPause(minDelay, maxDelay);
            element.SendKeys(text);
        }

        public static void HumanType(IWebElement element, string text, double minDelay = 0.03, double maxDelay = 0.15)
        {
            foreach (char c in text)
            {
                element.SendKeys(c.ToString());
                HumanPause(minDelay, maxDelay);
            }
        }

        public static void SmoothMoveToElement(IWebDriver driver, IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({block:'center'});", element);

            int steps = Random.Shared.Next(5, 10);

            var actions = new Actions(driver);

            for (int i = 0; i < steps; i++)
            {
                actions.MoveByOffset(
                    Random.Shared.Next(-15, 16),
                    Random.Shared.Next(-15, 16));

                actions.Pause(TimeSpan.FromMilliseconds(
                    Random.Shared.Next(20, 60)));
            }

            actions.MoveToElement(
                element,
                Random.Shared.Next(-4, 5),
                Random.Shared.Next(-4, 5));

            actions.Pause(TimeSpan.FromMilliseconds(
                Random.Shared.Next(100, 250)));

            actions.Perform();
        }
    }
}
