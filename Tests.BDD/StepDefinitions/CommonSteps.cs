using Locators_for_Web_Elements.Business.UI;
using Reqnroll;

namespace Locators_for_Web_Elements.Tests.BDD.StepDefinitions
{
    // Steps shared across every feature file. Kept in one place so the same
    // Gherkin phrasing ("I am on the EPAM home page", "I accept the cookies
    // banner") isn't redefined per feature, which would cause Reqnroll to
    // report an ambiguous step match.
    [Binding]
    public class CommonSteps
    {
        private readonly HomePage _homePage;

        public CommonSteps(HomePage homePage)
        {
            _homePage = homePage;
        }

        [Given(@"I am on the EPAM home page")]
        public void GivenIAmOnTheEpamHomePage()
        {
            // Navigation itself happens in the BeforeScenario hook (see
            // Support/Hooks.cs), exactly like TestsBase.BaseSetUp does for the
            // NUnit suite. This step exists so the precondition reads
            // explicitly in the feature file.
        }

        [Given(@"I accept the cookies banner")]
        public void GivenIAcceptTheCookiesBanner()
        {
            _homePage.AcceptCookies();
        }
    }
}
