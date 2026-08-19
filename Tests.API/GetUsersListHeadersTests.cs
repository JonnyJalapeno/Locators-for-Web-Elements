using System.Net;
using System.Threading;
using Locators_for_Web_Elements.Core.Api;

namespace Locators_for_Web_Elements.Tests.API
{
    // Task #2: Validate response header for a list of users.
    public class GetUsersListHeadersTests : ApiTestsBase
    {
        [Test]
        [CancelAfter(10_000)]
        [Description("GET /users returns a Content-Type header of application/json; charset=utf-8")]
        public async Task GetUsers_ReturnsExpectedContentTypeHeader(CancellationToken cancellationToken)
        {
            var response = await UsersApiClient.GetUsersAsync(cancellationToken);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "No error message is expected.");
            });

            // response.ContentType only exposes the MediaType part
            // (e.g. "application/json") - the charset parameter lives only in
            // the raw Content-Type header, so it has to be read from there.
            var contentTypeHeaderValue = response.GetHeaderValue("Content-Type");

            Assert.That(contentTypeHeaderValue, Is.Not.Null, "Content-Type header was not present in the response.");
            Assert.That(contentTypeHeaderValue, Is.EqualTo("application/json; charset=utf-8"));
        }
    }
}
