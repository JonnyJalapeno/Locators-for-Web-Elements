namespace Locators_for_Web_Elements.Tests
{
    [TestFixture]
    public class ServicesPageTests : TestsBase
    {
        [TestCase("Generative AI")]
        [TestCase("Responsible AI")]
        public void NavigatingToServiceCategory_DisplaysCorrectTitleAndRelatedExpertiseSection(
            string category)
        {
            var page = HomePage.AcceptCookies()
                .ClickServices()
                .SelectServiceCategory(category);

            Assert.That(page.GetTitle(), Does.Contain(category).IgnoreCase);
            Assert.That(page.IsRelatedExpertiseSectionDisplayed(), Is.True);
        }
    }
}
