using System.Net;
using System.Threading;

namespace Locators_for_Web_Elements.Tests.API
{
    // Task #5: Validate that a user is notified if a resource doesn't exist.
    public class GetInvalidEndpointTests : ApiTestsBase
    {
        [Test]
        [CancelAfter(10_000)]
        [Description("GET /invalidendpoint returns 404 Not Found")]
        public async Task GetInvalidEndpoint_ReturnsNotFound(CancellationToken cancellationToken)
        {
            var response = await UsersApiClient.GetAsync("/invalidendpoint", cancellationToken);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "No error message is expected.");
            });
        }
    }
}
