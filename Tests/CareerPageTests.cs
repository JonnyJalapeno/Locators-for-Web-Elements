namespace Locators_for_Web_Elements.Tests
{
    public class CareerPageTests : TestsBase
    {
        [TestCase("blockchain", "Serbia")]
        //[TestCase("python", "Uzbekistan")]
        public void CareerDescriptionContainsKeyword(string keyword, string country)
        {
            var page = HomePage.AcceptCookies()
            .ClickCareers()
            .ClickSearchCareers()
            .AcceptCookies()
            .SelectCountryFromDropdown(country)
            .ClickRemoteButton()
            .TypeIntoRoleOrKeywordSearch(keyword)
            .ClickTheSearchButton()
            .ExpandJobDescription();
            Assert.That(page.JobDescriptionContainsKeyword(keyword));
        }
    }
}
