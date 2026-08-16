namespace Locators_for_Web_Elements.Business.Api.Models
{
    // Body of a POST https://jsonplaceholder.typicode.com/users request.
    // Built step by step via CreateUserRequestBuilder rather than constructed
    // directly by callers/tests.
    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public Address? Address { get; set; }
        public Company? Company { get; set; }
    }
}
