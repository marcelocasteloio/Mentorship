using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IntegrationTests.Fixtures;
using IntegrationTests.Models;
using IntegrationTests.Models.Payloads;
using IntegrationTests.Models.Responses;
using IntegrationTests.Models.ViewModels;
using IntegrationTests.Services.Interfaces;

namespace IntegrationTests.Services;

public class CustomerService
    : ICustomerService
{
    // Constants
    public static readonly string REGISTER_NEW_CUSTOMER_PATH = RestApiClientFixture.BASE_URL + "/api/customers";

    // Fields
    private readonly HttpClient _httpClient;

    // Constructors
    public CustomerService(
        HttpClient httpClient
    )
    {
        _httpClient = httpClient;
    }

    // Public Methods
    public Task<HttpResponseMessage> RegisterNewCustomerAsync(
        RegisterNewCustomerPayload payload,
        CancellationToken cancellationToken
    )
    {
        return _httpClient.PostAsJsonAsync(
            REGISTER_NEW_CUSTOMER_PATH,
            payload,
            cancellationToken
        );
    }
    public Task<GetCustomerByIdResponse> GetCustomerByUriAsync(
        string uri, 
        CancellationToken cancellationToken
    )
    {
        var newUri = RestApiClientFixture.BASE_URL + uri;

        return _httpClient.GetFromJsonAsync<GetCustomerByIdResponse>(
            newUri,
            cancellationToken
        );
    }

}
