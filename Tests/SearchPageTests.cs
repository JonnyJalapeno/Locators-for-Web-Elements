namespace Locators_for_Web_Elements.Tests
{
    public class SearchPageTests : TestsBase
    {
        [TestCase("BLOCKCHAIN")]
        //[TestCase("Cloud")]
        //[TestCase("Automation")]
        public void SearchContainsKeyword(string s1)
        {
            var page = HomePage.AcceptCookies()
            .ClickMagnifierSearch()
            .InputPhraseIntoMagnifierSearch(s1)
            .ClickFindButton();
            Assert.That(page.CheckAllLinksForSearchTerm(s1));
        }
    }
}
