namespace Locators_for_Web_Elements.Tests
{
    public class SearchPageTests : TestsBase
    {
        [TestCase("BLOCKCHAIN")]
        //[TestCase("Cloud")]
        //[TestCase("Automation")]
        public void SearchContainsKeyword(string searchWord)
        {
            var page = HomePage.AcceptCookies()
            .ClickMagnifierSearch()
            .InputPhraseIntoMagnifierSearch(searchWord)
            .ClickFindButton();
            Assert.That(page.CheckAllLinksForSearchTerm(searchWord));
        }
    }
}
