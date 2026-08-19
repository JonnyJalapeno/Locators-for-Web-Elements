using System.Net;
using System.Threading;

namespace Locators_for_Web_Elements.Tests.API
{
    // Task #3: Validate response body content for a list of users - exactly
    // 10 users, unique IDs, non-empty Name/Username, non-empty Company Name.
    public class GetUsersListContentTests : ApiTestsBase
    {
        [Test]
        [CancelAfter(10_000)]
        [Description("GET /users returns exactly 10 unique, well-formed users")]
        public async Task GetUsers_ReturnsTenUniqueUsersWithValidData(CancellationToken cancellationToken)
        {
            var response = await UsersApiClient.GetUsersAsync(cancellationToken);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "No error message is expected.");
                Assert.That(response.Data, Is.Not.Null);
            });

            var users = response.Data!;

            Assert.That(users, Has.Count.EqualTo(10), "Response body is expected to contain an array of 10 users.");

            var distinctIdsCount = users.Select(u => u.Id).Distinct().Count();
            Assert.That(distinctIdsCount, Is.EqualTo(users.Count), "Each user is expected to have a different ID.");

            Assert.Multiple(() =>
            {
                foreach (var user in users)
                {
                    Assert.That(user.Name, Is.Not.Null.And.Not.Empty, $"User {user.Id} is expected to have a non-empty Name.");
                    Assert.That(user.Username, Is.Not.Null.And.Not.Empty, $"User {user.Id} is expected to have a non-empty Username.");
                    Assert.That(user.Company.Name, Is.Not.Null.And.Not.Empty, $"User {user.Id} is expected to have a non-empty Company Name.");
                }
            });
        }
    }
}
