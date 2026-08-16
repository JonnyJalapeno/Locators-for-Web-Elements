using RestSharp;

namespace Locators_for_Web_Elements.Core
{
    // Builder Design Pattern: lets callers assemble a (potentially complex)
    // RestRequest step by step - resource, method, headers, query/url
    // parameters, body, timeout - instead of juggling RestRequest's own
    // constructor/mutators directly. Business-layer API clients compose this
    // builder to keep request-construction code fluent and readable.
    public class RestRequestBuilder
    {
        private readonly RestRequest _request;

        private RestRequestBuilder(string resource, Method method)
        {
            _request = new RestRequest(resource, method);
        }

        public static RestRequestBuilder For(string resource, Method method) => new(resource, method);

        public RestRequestBuilder WithHeader(string name, string value)
        {
            _request.AddHeader(name, value);
            return this;
        }

        public RestRequestBuilder WithQueryParameter(string name, string value)
        {
            _request.AddQueryParameter(name, value);
            return this;
        }

        public RestRequestBuilder WithUrlSegment(string name, string value)
        {
            _request.AddUrlSegment(name, value);
            return this;
        }

        public RestRequestBuilder WithJsonBody<T>(T body) where T : class
        {
            _request.AddJsonBody(body);
            return this;
        }

        public RestRequestBuilder WithTimeout(TimeSpan timeout)
        {
            _request.Timeout = timeout;
            return this;
        }

        public RestRequest Build() => _request;
    }
}
