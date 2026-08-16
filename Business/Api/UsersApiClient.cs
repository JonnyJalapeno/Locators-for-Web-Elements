using Locators_for_Web_Elements.Core;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Locators_for_Web_Elements.Business
{
    // Business-layer wrapper around the "/users" resource of
    // https://jsonplaceholder.typicode.com. Uses the Core IApiClient for
    // transport + logging and Core's RestRequestBuilder (Builder Design
    // Pattern) to assemble each RestRequest step by step.
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

        public Task<ApiResponse<List<User>>> GetUsersAsync()
        {
            _logger.LogInformation("Preparing GET request for the list of users");

            var request = RestRequestBuilder
                .For(UsersResource, Method.Get)
                .WithHeader("Accept", "application/json")
                .Build();

            return _apiClient.ExecuteAsync<List<User>>(request, "Get list of users");
        }

        public Task<ApiResponse<CreateUserResponse>> CreateUserAsync(CreateUserRequest newUser)
        {
            _logger.LogInformation("Preparing POST request to create user '{Username}'", newUser.Username);

            var request = RestRequestBuilder
                .For(UsersResource, Method.Post)
                .WithHeader("Accept", "application/json")
                .WithJsonBody(newUser)
                .Build();

            return _apiClient.ExecuteAsync<CreateUserResponse>(request, "Create user");
        }

        // Generic GET, used for negative-path checks such as hitting a
        // resource that does not exist.
        public Task<ApiResponse<object>> GetAsync(string resource)
        {
            _logger.LogInformation("Preparing GET request for resource '{Resource}'", resource);

            var request = RestRequestBuilder
                .For(resource, Method.Get)
                .Build();

            return _apiClient.ExecuteAsync(request, $"GET {resource}");
        }
    }
}
