using System.Net;
using Locators_for_Web_Elements.Core.Api;

namespace Locators_for_Web_Elements.Tests.API
{
    // Task #2: Validate response header for a list of users.
    public class GetUsersListHeadersTests : ApiTestsBase
    {
        [Test]
        [Description("GET /users returns a Content-Type header of application/json; charset=utf-8")]
        public async Task GetUsers_ReturnsExpectedContentTypeHeader()
        {
            var response = await UsersApiClient.GetUsersAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "No error message is expected.");
            });

            var contentTypeHeaderValue = response.GetHeaderValue("Content-Type");

            Assert.That(contentTypeHeaderValue, Is.Not.Null, "Content-Type header was not present in the response.");
            Assert.That(contentTypeHeaderValue, Is.EqualTo("application/json; charset=utf-8"));
        }
    }
}
