using RestSharp;

namespace Locators_for_Web_Elements.Core
{
    public interface IApiClient
    {
        // Executes a request and deserializes the body into T.
        Task<ApiResponse<T>> ExecuteAsync<T>(RestRequest request, string actionDescription);

        // Executes a request without deserialization - useful for endpoints
        // that are only checked for status code / headers (e.g. a 404 check).
        Task<ApiResponse<object>> ExecuteAsync(RestRequest request, string actionDescription);
    }
}
