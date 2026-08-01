namespace Locators_for_Web_Elements.Tests
{
    [TestFixture]
    public class CarouselArticleTitleTests : TestsBase
    {
        [TestCase(3)]
        public void ArticleTitle_MatchesCarouselSlideTitle_AfterSwiping(int numberOfSwipes)
        {
            var insightPage = HomePage.AcceptCookies().ClickInsights();

            insightPage.Carousel.Swipe(numberOfSwipes);
            var expectedTitle = insightPage.Carousel.GetActiveSlideTitle();

            var actualTitle = insightPage.Carousel.ClickReadMoreOnActiveSlide().GetTitle();

            Assert.That(actualTitle, Is.EqualTo(expectedTitle));
        }
    }
}
