namespace Locators_for_Web_Elements.Core
{
    // Bound from the "Api" section of appsettings.json. Kept generic/agnostic of
    // any particular service under test - the concrete base URL for a given
    // system (e.g. JSONPlaceholder) is supplied through configuration, not code.
    public class ApiConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
    }
}
