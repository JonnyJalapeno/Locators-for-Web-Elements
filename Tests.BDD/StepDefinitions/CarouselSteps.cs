using Locators_for_Web_Elements.Business;
using Reqnroll;

namespace Locators_for_Web_Elements.Tests.BDD.StepDefinitions
{
    [Binding]
    public class CarouselSteps
    {
        private readonly HomePage _homePage;
        private InsightsPage? _insightsPage;
        private string? _expectedTitle;
        private string? _actualTitle;

        public CarouselSteps(HomePage homePage)
        {
            _homePage = homePage;
        }

        [When(@"I navigate to the Insights page")]
        public void WhenINavigateToTheInsightsPage()
        {
            _insightsPage = _homePage.ClickInsights();
        }

        [When(@"I swipe the carousel (\d+) times?")]
        public void WhenISwipeTheCarouselTimes(int numberOfSwipes)
        {
            Assert.That(_insightsPage, Is.Not.Null);
            _insightsPage!.Carousel.Swipe(numberOfSwipes);
            _expectedTitle = _insightsPage.Carousel.GetActiveSlideTitle();
        }

        [When(@"I click ""Read More"" on the active carousel slide")]
        public void WhenIClickReadMoreOnTheActiveCarouselSlide()
        {
            Assert.That(_insightsPage, Is.Not.Null);
            _actualTitle = _insightsPage!.Carousel.ClickReadMoreOnActiveSlide().GetTitle();
        }

        [Then(@"the article title should match the active slide title")]
        public void ThenTheArticleTitleShouldMatchTheActiveSlideTitle()
        {
            Assert.That(_actualTitle, Is.EqualTo(_expectedTitle));
        }
    }
}
