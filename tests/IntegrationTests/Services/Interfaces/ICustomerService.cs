using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IntegrationTests.Models;
using IntegrationTests.Models.Payloads;
using IntegrationTests.Models.Responses;

namespace IntegrationTests.Services.Interfaces;

public interface ICustomerService
{
    Task<HttpResponseMessage> RegisterNewCustomerAsync(
        RegisterNewCustomerPayload payload,
        CancellationToken cancellationToken
    );
    Task<GetCustomerByIdResponse> GetCustomerByUriAsync(
        string uri,
        CancellationToken cancellationToken
    );
}
