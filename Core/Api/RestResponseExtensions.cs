using RestSharp;

namespace Locators_for_Web_Elements.Core.Api
{
    public static class RestResponseExtensions
    {
        // Looks up a header by name across both the transport headers and the
        // content headers collections (RestSharp splits them, e.g. Content-Type
        // usually lands in ContentHeaders). Works on both RestResponse and
        // RestResponse<T>, since both derive from RestResponseBase.
        public static string? GetHeaderValue(this RestResponseBase response, string name) =>
            response.Headers.Concat(response.ContentHeaders)
                .FirstOrDefault(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase))
                ?.Value?.ToString();
    }
}
