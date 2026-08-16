using RestSharp;

namespace Locators_for_Web_Elements.Core
{
    // Thin, framework-owned wrapper around a RestSharp response so that the
    // Business layer (and tests) never need to reference RestSharp types
    // directly - they only depend on Core abstractions.
    public class ApiResponse<T>
    {
        public required System.Net.HttpStatusCode StatusCode { get; init; }
        public bool IsSuccessful { get; init; }
        public T? Data { get; init; }
        public string? Content { get; init; }
        public string? ContentType { get; init; }
        public string? ErrorMessage { get; init; }
        public IReadOnlyCollection<HeaderParameter> Headers { get; init; } = Array.Empty<HeaderParameter>();
        public IReadOnlyCollection<HeaderParameter> ContentHeaders { get; init; } = Array.Empty<HeaderParameter>();

        // Looks up a header by name across both the transport headers and the
        // content headers collections (RestSharp splits them, e.g. Content-Type
        // usually lands in ContentHeaders).
        public string? GetHeaderValue(string name) =>
            Headers.Concat(ContentHeaders)
                .FirstOrDefault(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase))
                ?.Value?.ToString();
    }
}
