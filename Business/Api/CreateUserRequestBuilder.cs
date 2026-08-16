using Locators_for_Web_Elements.Business.Api.Models;

namespace Locators_for_Web_Elements.Business.Api
{
    // Builder Design Pattern applied at the Business (domain) level: lets a
    // test compose a new-user payload one field at a time - only Name and
    // Username are required by the "create user" tests below, but the
    // builder also supports the optional fields the real API accepts, so the
    // same fluent API scales to more complex request bodies without changing
    // call sites.
    public class CreateUserRequestBuilder
    {
        private readonly CreateUserRequest _request = new();

        public CreateUserRequestBuilder WithName(string name)
        {
            _request.Name = name;
            return this;
        }

        public CreateUserRequestBuilder WithUsername(string username)
        {
            _request.Username = username;
            return this;
        }

        public CreateUserRequestBuilder WithEmail(string email)
        {
            _request.Email = email;
            return this;
        }

        public CreateUserRequestBuilder WithPhone(string phone)
        {
            _request.Phone = phone;
            return this;
        }

        public CreateUserRequestBuilder WithWebsite(string website)
        {
            _request.Website = website;
            return this;
        }

        public CreateUserRequestBuilder WithAddress(Address address)
        {
            _request.Address = address;
            return this;
        }

        public CreateUserRequestBuilder WithCompany(Company company)
        {
            _request.Company = company;
            return this;
        }

        public CreateUserRequest Build() => _request;
    }
}
