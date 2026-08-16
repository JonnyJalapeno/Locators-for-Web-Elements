using Microsoft.Extensions.Logging;
using RestSharp;

namespace Locators_for_Web_Elements.Core
{
    // Base API client for the whole TAF. Every API test/business client goes
    // through this class, which guarantees that every outgoing request and
    // every incoming response is logged (Info for the summary line, Debug for
    // full body dumps, Error when the call fails), regardless of which
    // endpoint or Business-layer class is calling it.
    public class ApiClient : IApiClient
    {
        private readonly IRestClient _client;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(IRestClient client, ILogger<ApiClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>(RestRequest request, string actionDescription)
        {
            LogRequest(request, actionDescription);
            var response = await _client.ExecuteAsync<T>(request);
            LogResponse(response, actionDescription);

            return new ApiResponse<T>
            {
                StatusCode = response.StatusCode,
                IsSuccessful = response.IsSuccessful,
                Data = response.Data,
                Content = response.Content,
                ContentType = response.ContentType,
                ErrorMessage = response.ErrorMessage,
                Headers = response.Headers?.ToList() ?? new List<HeaderParameter>(),
                ContentHeaders = response.ContentHeaders?.ToList() ?? new List<HeaderParameter>()
            };
        }

        public async Task<ApiResponse<object>> ExecuteAsync(RestRequest request, string actionDescription)
        {
            LogRequest(request, actionDescription);
            var response = await _client.ExecuteAsync(request);
            LogResponse(response, actionDescription);

            return new ApiResponse<object>
            {
                StatusCode = response.StatusCode,
                IsSuccessful = response.IsSuccessful,
                Content = response.Content,
                ContentType = response.ContentType,
                ErrorMessage = response.ErrorMessage,
                Headers = response.Headers?.ToList() ?? new List<HeaderParameter>(),
                ContentHeaders = response.ContentHeaders?.ToList() ?? new List<HeaderParameter>()
            };
        }

        private void LogRequest(RestRequest request, string actionDescription)
        {
            var url = _client.BuildUri(request);
            _logger.LogInformation(
                "API action '{Action}': sending {Method} request to {Url}",
                actionDescription, request.Method, url);

            var bodyParameter = request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);
            if (bodyParameter is not null)
            {
                _logger.LogDebug("API action '{Action}': request body: {Body}", actionDescription, bodyParameter.Value);
            }
        }

        private void LogResponse(RestResponseBase response, string actionDescription)
        {
            if (response.IsSuccessful)
            {
                _logger.LogInformation(
                    "API action '{Action}': received {StatusCode} ({StatusCodeNumeric})",
                    actionDescription, response.StatusCode, (int)response.StatusCode);
            }
            else
            {
                _logger.LogError(
                    "API action '{Action}': received {StatusCode} ({StatusCodeNumeric}). Error: {Error}",
                    actionDescription, response.StatusCode, (int)response.StatusCode,
                    response.ErrorMessage ?? response.Content);
            }

            _logger.LogDebug("API action '{Action}': response content: {Content}", actionDescription, response.Content);
        }
    }
}
