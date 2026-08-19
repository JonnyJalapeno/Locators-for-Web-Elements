using System.Net;
using System.Threading;

namespace Locators_for_Web_Elements.Tests.API
{
    // Task #1: Validate that the list of users can be received successfully.
    public class GetUsersListTests : ApiTestsBase
    {
        [Test]
        [CancelAfter(10_000)]
        [Description("GET /users returns 200 OK with users containing all expected fields")]
        public async Task GetUsers_ReturnsUsersWithExpectedFields(CancellationToken cancellationToken)
        {
            var response = await UsersApiClient.GetUsersAsync(cancellationToken);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.IsSuccessful, Is.True);
                Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "No error message is expected.");
                Assert.That(response.Data, Is.Not.Null.And.Not.Empty);
            });

            Assert.Multiple(() =>
            {
                foreach (var user in response.Data!)
                {
                    Assert.That(user.Id, Is.GreaterThan(0), "id should be present.");
                    Assert.That(user.Name, Is.Not.Null.And.Not.Empty, "name should be present.");
                    Assert.That(user.Username, Is.Not.Null.And.Not.Empty, "username should be present.");
                    Assert.That(user.Email, Is.Not.Null.And.Not.Empty, "email should be present.");
                    Assert.That(user.Address, Is.Not.Null, "address should be present.");
                    Assert.That(user.Phone, Is.Not.Null.And.Not.Empty, "phone should be present.");
                    Assert.That(user.Website, Is.Not.Null.And.Not.Empty, "website should be present.");
                    Assert.That(user.Company, Is.Not.Null, "company should be present.");
                }
            });
        }
    }
}
