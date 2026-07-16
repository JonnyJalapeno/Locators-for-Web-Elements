using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Locators_for_Web_Elements
{
    public class SearchPage : BaseComponent<SearchPage>
    {

        private readonly By ArticleLinks = By.TagName("a");
        private readonly By ArticleParagraphs = By.TagName("p");
        private readonly By SearchResultContainer = By.XPath("//div[contains(@class, 'search-results__items')]");
        private readonly By SearchResultMore = By.XPath("//a[contains(@class,'search-results__view-more') and not(contains(concat(' ', normalize-space(@class), ' '), ' hidden '))]");
        private readonly By Footer = By.XPath("//footer[contains(@class,'search-results__footer')]");
        private readonly By Article = By.XPath("//article[contains(@class, 'search-results__item')]");

        public SearchPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
        }

        public bool CheckLinksForSearchTerm(string phrase)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(phrase);
            var links = Wait.Until(driver =>
            {
                var container = FindElementByLocator(SearchResultContainer);
                var links = FindElementsByLocator(ArticleLinks, container).ToList();
                return links.Count > 0 ? links : null;
            });
            return links.All(element => ElementContainsPhrase(element, phrase));
        }

        public bool CheckAllLinksForSearchTerm(string phrase)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(phrase);
            var footer = Driver.FindElement(Footer);

            new Actions(Driver).ScrollToElement(footer).Perform();

            Driver.FindElement(SearchResultMore).Click();

            var articles = FetchArticles();

            return AllArticlesContainPhrase(articles, phrase);
        }

        private List<IWebElement> FetchArticles()
        {
            return Wait.Until(driver =>
            {
                var container = FindElementByLocator(SearchResultContainer);
                var articles = FindElementsByLocator(Article, container).ToList();
                return articles.Count > 0 ? articles : null;
            });
        }

        private bool AllArticlesContainPhrase(IEnumerable<IWebElement> articles, string phrase)
        {
            return articles.All(element =>
            {
                var linkText = element.FindElement(ArticleLinks);
                var paragraphText = element.FindElement(ArticleParagraphs);

                return ElementContainsPhrase(linkText, phrase) ||
                       ElementContainsPhrase(paragraphText, phrase);
            });
        }
    }
}
