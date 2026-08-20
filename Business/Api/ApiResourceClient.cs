using System.Threading;
using Locators_for_Web_Elements.Core.Api;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Locators_for_Web_Elements.Business.Api
{
    public class ApiResourceClient
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<ApiResourceClient> _logger;

        public ApiResourceClient(
            IApiClient apiClient,
            ILogger<ApiResourceClient> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public Task<RestResponse> GetAsync(
            string resource,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Preparing GET request for resource '{Resource}'",
                resource);

            var request = new RestRequest(resource, Method.Get);

            return _apiClient.ExecuteAsync(
                request,
                $"GET {resource}",
                cancellationToken);
        }
    }
}