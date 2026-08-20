using Locators_for_Web_Elements.Business.UI;
using Reqnroll;

namespace Locators_for_Web_Elements.Tests.BDD.StepDefinitions
{
    [Binding]
    public class ServicesNavigationSteps
    {
        private readonly HomePage _homePage;
        private ServicesPage? _servicesPage;

        public ServicesNavigationSteps(HomePage homePage)
        {
            _homePage = homePage;
        }

        [When(@"I open the ""Services"" navigation menu")]
        public void WhenIOpenTheServicesNavigationMenu()
        {
            _homePage.ClickServices();
        }

        [When(@"I select the ""(.*)"" service category")]
        public void WhenISelectTheServiceCategory(string category)
        {
            _servicesPage = _homePage.SelectServiceCategory(category);
        }

        [Then(@"the page title should contain ""(.*)""")]
        public void ThenThePageTitleShouldContain(string expectedTitleKeyword)
        {
            Assert.That(_servicesPage, Is.Not.Null, "Navigated to no service category page yet.");
            Assert.That(_servicesPage!.GetTitle(), Does.Contain(expectedTitleKeyword).IgnoreCase);
        }

        [Then(@"the ""Our Related Expertise"" section should be displayed")]
        public void ThenTheOurRelatedExpertiseSectionShouldBeDisplayed()
        {
            Assert.That(_servicesPage, Is.Not.Null, "Navigated to no service category page yet.");
            Assert.That(_servicesPage!.IsRelatedExpertiseSectionDisplayed(), Is.True);
        }
    }
}
