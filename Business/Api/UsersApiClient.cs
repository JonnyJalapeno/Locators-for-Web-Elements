using System.Threading;
using Locators_for_Web_Elements.Business.Api.Models;
using Locators_for_Web_Elements.Core.Api;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Locators_for_Web_Elements.Business.Api
{
    // Business-layer wrapper around the "/users" resource of
    // https://jsonplaceholder.typicode.com. Uses Core's IApiClient for
    // transport + logging. Requests are built directly with RestSharp's own
    // RestRequest - its Add*() methods are already fluent/chainable, so no
    // extra builder wrapper is needed on top of them. Returns RestSharp's own
    // RestResponse<T>/RestResponse - this class isn't RestSharp-agnostic (it
    // references Method/RestRequest directly), it just centralizes knowledge
    // of the "/users" resource path.
    //
    // No Accept header is set explicitly: RestSharp sets it automatically
    // based on the registered serializers, so adding it manually is
    // redundant (see https://restsharp.dev/v107/#automatic-json-and-xml-requests).
    public class UsersApiClient
    {
        private const string UsersResource = "/users";

        private readonly IApiClient _apiClient;
        private readonly ILogger<UsersApiClient> _logger;

        public UsersApiClient(IApiClient apiClient, ILogger<UsersApiClient> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public Task<RestResponse<List<User>>> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Preparing GET request for the list of users");

            var request = new RestRequest(UsersResource, Method.Get);

            return _apiClient.ExecuteAsync<List<User>>(request, "Get list of users", cancellationToken);
        }

        public Task<RestResponse<CreateUserResponse>> CreateUserAsync(
            CreateUserRequest newUser,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Preparing POST request to create user '{Username}'", newUser.Username);

            var request = new RestRequest(UsersResource, Method.Post)
                .AddJsonBody(newUser);

            return _apiClient.ExecuteAsync<CreateUserResponse>(request, "Create user", cancellationToken);
        }

        // Generic GET, used for negative-path checks such as hitting a
        // resource that does not exist.
        public Task<RestResponse> GetAsync(string resource, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Preparing GET request for resource '{Resource}'", resource);

            var request = new RestRequest(resource, Method.Get);

            return _apiClient.ExecuteAsync(request, $"GET {resource}", cancellationToken);
        }
    }
}
