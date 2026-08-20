using Locators_for_Web_Elements.Business.UI;
using Reqnroll;

namespace Locators_for_Web_Elements.Tests.BDD.StepDefinitions
{
    [Binding]
    public class SearchSteps
    {
        private readonly HomePage _homePage;
        private SearchPage? _searchPage;

        public SearchSteps(HomePage homePage)
        {
            _homePage = homePage;
        }

        [When(@"I open the site search")]
        public void WhenIOpenTheSiteSearch()
        {
            _homePage.ClickMagnifierSearch();
        }

        [When(@"I search for ""(.*)""")]
        public void WhenISearchFor(string searchWord)
        {
            _homePage.InputPhraseIntoMagnifierSearch(searchWord);
            _searchPage = _homePage.ClickFindButton();
        }

        [Then(@"all search results should relate to ""(.*)""")]
        public void ThenAllSearchResultsShouldRelateTo(string searchWord)
        {
            Assert.That(_searchPage, Is.Not.Null);
            Assert.That(_searchPage!.CheckAllLinksForSearchTerm(searchWord), Is.True);
        }
    }
}
