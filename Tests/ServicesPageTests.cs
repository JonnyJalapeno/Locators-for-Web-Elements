namespace Locators_for_Web_Elements.Tests
{
    [TestFixture]
    public class ServicesPageTests : TestsBase
    {
        [TestCase("Generative AI", "Generative AI")]
        [TestCase("Responsible AI", "Responsible AI")]
        public void NavigatingToServiceCategory_DisplaysCorrectTitleAndRelatedExpertiseSection(
            string category, string expectedTitleKeyword)
        {
            var page = HomePage.AcceptCookies()
                .ClickServices()
                .SelectServiceCategory(category);

            Assert.That(page.GetTitle(), Does.Contain(expectedTitleKeyword).IgnoreCase);
            Assert.That(page.IsRelatedExpertiseSectionDisplayed(), Is.True);
        }
    }
}
