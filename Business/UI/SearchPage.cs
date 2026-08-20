using Locators_for_Web_Elements.Core.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Locators_for_Web_Elements.Business.UI
{
    public class SearchPage
    {
        private readonly IWebDriver Driver;
        private readonly IElementInteractor Interactor;

        private readonly By ArticleLinks = By.TagName("a");
        private readonly By ArticleParagraphs = By.TagName("p");
        private readonly By SearchResultContainer = By.XPath("//div[contains(@class, 'search-results__items')]");
        private readonly By SearchResultMore = By.XPath("//a[contains(@class,'search-results__view-more') and not(contains(concat(' ', normalize-space(@class), ' '), ' hidden '))]");
        private readonly By Footer = By.XPath("//footer[contains(@class,'search-results__footer')]");
        private readonly By Article = By.XPath("//article[contains(@class, 'search-results__item')]");
        private readonly By Preloader = By.XPath("//div[contains(@class, 'preloader')]");

        public SearchPage(IWebDriver driver, IElementInteractor interactor)
        {
            Driver = driver;
            Interactor = interactor;
        }

        public bool CheckLinksForSearchTerm(string phrase)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(phrase);
            var links = Interactor.FindContainerAndReturnItsElements(SearchResultContainer, ArticleLinks);
            return links.All(element => Interactor.ElementContainsPhrase(element, phrase));
        }

        public bool CheckAllLinksForSearchTerm(string phrase)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(phrase);
            Interactor.WaitForUrlToContain("search");
            Interactor.WaitForPreloaderToDisappear(Preloader);
            var footer = Interactor.FindPresentElementByLocator(Footer);

            new Actions(Driver).ScrollToElement(footer).Perform();

            Interactor.FindElementByLocator(SearchResultMore).Click();

            var articles = FetchArticles();

            return AllArticlesContainPhrase(articles, phrase);
        }

        private List<IWebElement> FetchArticles()
        {
            Interactor.WaitForPreloaderToDisappear(Preloader);
            return Interactor.FindContainerAndReturnItsElements(SearchResultContainer, Article);
        }

        private bool AllArticlesContainPhrase(IEnumerable<IWebElement> articles, string phrase)
        {
            return articles.All(element =>
            {
                var linkText = element.FindElement(ArticleLinks);
                var paragraphText = element.FindElement(ArticleParagraphs);

                return Interactor.ElementContainsPhrase(linkText, phrase) ||
                       Interactor.ElementContainsPhrase(paragraphText, phrase);
            });
        }
    }
}
