using System;
using System.Net.Http;
using FluentAssertions;
using IntegrationTests.Models.Payloads;
using IntegrationTests.Models.Responses;
using IntegrationTests.Services;
using IntegrationTests.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.Fixtures;

[CollectionDefinition(nameof(RestApiClientFixture))]
public class RestApiClientFixtureCollection
    : ICollectionFixture<RestApiClientFixture>
{
}

public class RestApiClientFixture
    : IDisposable
{
    // Constants
    public const string BASE_URL = "http://localhost:5000";

    // Properties
    public ServiceProvider ServiceProvider { get; }

    // Constructors
    public RestApiClientFixture()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);

        ServiceProvider = services.BuildServiceProvider();
    }

    // Public Static Methods
    private static void ConfigureServices(IServiceCollection services)
    {
        services
            .AddScoped<ICustomerService, CustomerService>();
        
        services
            .AddHttpClient<CustomerService>();
    }
    public static void ValidateStatusCode(HttpResponseMessage response, int actualStatusCode)
    {
        response.StatusCode.Should().Be(actualStatusCode);
    }

    // Compare Methods
    public static bool Compare(RegisterNewCustomerPayload registerNewCustomerPayload, RegisterNewCustomerResponse registerNewCustomerResponse)
    {
        return 
            registerNewCustomerPayload.FirstName == registerNewCustomerResponse.Data.FirstName &&
            registerNewCustomerPayload.LastName == registerNewCustomerResponse.Data.LastName &&
            registerNewCustomerPayload.BirthDate == registerNewCustomerResponse.Data.BirthDate &&
            registerNewCustomerPayload.Email == registerNewCustomerResponse.Data.Email;
    }
    public static bool Compare(RegisterNewCustomerResponse registerNewCustomerResponse, GetCustomerByIdResponse getCustomerByIdResponse)
    {
        return 
            registerNewCustomerResponse.Data.Id == getCustomerByIdResponse.Data.Id &&
            registerNewCustomerResponse.Data.FirstName == getCustomerByIdResponse.Data.FirstName &&
            registerNewCustomerResponse.Data.LastName == getCustomerByIdResponse.Data.LastName &&
            registerNewCustomerResponse.Data.BirthDate == getCustomerByIdResponse.Data.BirthDate &&
            registerNewCustomerResponse.Data.Email == getCustomerByIdResponse.Data.Email;
    }

    public void Dispose()
    {
        
    }
}
