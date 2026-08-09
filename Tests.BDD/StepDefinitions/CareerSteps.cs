using Locators_for_Web_Elements.Business;
using Reqnroll;

namespace Locators_for_Web_Elements.Tests.BDD.StepDefinitions
{
    [Binding]
    public class CareerSteps
    {
        private readonly HomePage _homePage;
        private CareerPage? _careerPage;

        public CareerSteps(HomePage homePage)
        {
            _homePage = homePage;
        }

        [When(@"I go to the Careers page")]
        public void WhenIGoToTheCareersPage()
        {
            _careerPage = _homePage.ClickCareers();
        }

        [When(@"I click ""Search Careers"" on the Careers page")]
        public void WhenIClickSearchCareers()
        {
            Assert.That(_careerPage, Is.Not.Null);
            _careerPage = _careerPage!.ClickSearchCareers();
        }

        [When(@"I accept the cookies banner on the Careers page")]
        public void WhenIAcceptTheCookiesBannerOnTheCareersPage()
        {
            Assert.That(_careerPage, Is.Not.Null);
            _careerPage = _careerPage!.AcceptCookies();
        }

        [When(@"I select ""(.*)"" from the country dropdown")]
        public void WhenISelectFromTheCountryDropdown(string country)
        {
            Assert.That(_careerPage, Is.Not.Null);
            _careerPage = _careerPage!.SelectCountryFromDropdown(country);
        }

        [When(@"I filter jobs by ""Remote"" workplace type")]
        public void WhenIFilterJobsByRemoteWorkplaceType()
        {
            Assert.That(_careerPage, Is.Not.Null);
            _careerPage = _careerPage!.ClickRemoteButton();
        }

        [When(@"I search careers for the keyword ""(.*)""")]
        public void WhenISearchCareersForTheKeyword(string keyword)
        {
            Assert.That(_careerPage, Is.Not.Null);
            _careerPage = _careerPage!
                .TypeIntoRoleOrKeywordSearch(keyword)
                .ClickTheSearchButton();
        }

        [When(@"I expand the job description")]
        public void WhenIExpandTheJobDescription()
        {
            Assert.That(_careerPage, Is.Not.Null);
            _careerPage = _careerPage!.ExpandJobDescription();
        }

        [Then(@"the job description should contain the keyword ""(.*)""")]
        public void ThenTheJobDescriptionShouldContainTheKeyword(string keyword)
        {
            Assert.That(_careerPage, Is.Not.Null);
            Assert.That(_careerPage!.JobDescriptionContainsKeyword(keyword), Is.True);
        }
    }
}
