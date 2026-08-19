using System.Net;
using System.Threading;
using Locators_for_Web_Elements.Business.Api;

namespace Locators_for_Web_Elements.Tests.API
{
    // Task #4: Validate that a user can be created.
    public class CreateUserTests : ApiTestsBase
    {
        [Test]
        [CancelAfter(10_000)]
        [Description("POST /users with Name and Username returns 201 Created and an ID")]
        public async Task CreateUser_ReturnsCreatedUserWithId(CancellationToken cancellationToken)
        {
            var newUser = new CreateUserRequestBuilder()
                .WithName("John Tester")
                .WithUsername("john.tester")
                .Build();

            var response = await UsersApiClient.CreateUserAsync(newUser, cancellationToken);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.ErrorMessage, Is.Null.Or.Empty, "No error message is expected.");
                Assert.That(response.Data, Is.Not.Null, "Response body is not expected to be empty.");
                Assert.That(response.Data!.Id, Is.GreaterThan(0), "Response body is expected to contain the ID value.");
            });
        }
    }
}
