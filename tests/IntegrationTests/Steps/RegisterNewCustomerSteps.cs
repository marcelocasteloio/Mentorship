using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IntegrationTests.Factories;
using IntegrationTests.Fixtures;
using IntegrationTests.Models;
using IntegrationTests.Models.Payloads;
using IntegrationTests.Models.Responses;
using IntegrationTests.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace IntegrationTests.Steps
{
    [Binding]
    [Collection(nameof(RestApiClientFixture))]
    public sealed class RegisterNewCustomerSteps
    {
        // Constant
        public const string REGISTER_NEW_CUSTOMER_PAYLOAD_KEY = "REGISTER_NEW_CUSTOMER_PAYLOAD_KEY";
        public const string REGISTER_NEW_CUSTOMER_RESPONSE_KEY = "REGISTER_NEW_CUSTOMER_RESPONSE_KEY";
        public const string REGISTER_NEW_CUSTOMER_RESPONSE_OBJECT_KEY = "REGISTER_NEW_CUSTOMER_RESPONSE_OBJECT_KEY";

        // Fields
        private readonly CancellationTokenSource _cts = new();
        private readonly ScenarioContext _scenarioContext;
        private readonly RestApiClientFixture _restApiClientFixture;
        private readonly ICustomerService _customerService;

        // Constructors
        public RegisterNewCustomerSteps(
            ScenarioContext scenarioContext,
            RestApiClientFixture restApiClientFixture
        )
        {
            _scenarioContext = scenarioContext;
            _restApiClientFixture = restApiClientFixture;

            var scopedServiceProvider = _restApiClientFixture.ServiceProvider.CreateScope().ServiceProvider;
            _customerService = scopedServiceProvider.GetRequiredService<ICustomerService>();
        }

        // Public Methods
        [Given(@"o payload para o registro de cliente:")]
        public void DadoOPayloadParaORegistroDeCliente(Table table)
        {
            var registerNewCustomerTableItem = table.CreateInstance<RegisterNewCustomerTable>();
            var payload = RegisterNewCustomerPayloadFactory.Create(registerNewCustomerTableItem);

            _scenarioContext.Add(REGISTER_NEW_CUSTOMER_PAYLOAD_KEY, payload);
        }

        [When(@"a request de registro de cliente eh feita")]
        public async Task QuandoARequestDeRegistroDeClienteEhFeita()
        {
            var payload = _scenarioContext.Get<RegisterNewCustomerPayload>(REGISTER_NEW_CUSTOMER_PAYLOAD_KEY);

            var response = await _customerService.RegisterNewCustomerAsync(
                payload,
                _cts.Token
            );

            var registerNewCustomerResponse = await response.Content.ReadFromJsonAsync<RegisterNewCustomerResponse>(_cts.Token);

            _scenarioContext.Add(REGISTER_NEW_CUSTOMER_RESPONSE_KEY, response);
            _scenarioContext.Add(REGISTER_NEW_CUSTOMER_RESPONSE_OBJECT_KEY, registerNewCustomerResponse);
        }

        [Then(@"o status code da response de registro de cliente deve ser (.*)")]
        public void EntaoOStatusCodeDaResponseDeRegistroDeClienteDeveSer(int statusCode)
        {
            RestApiClientFixture.ValidateStatusCode(
                _scenarioContext.Get<HttpResponseMessage>(REGISTER_NEW_CUSTOMER_RESPONSE_KEY),
                statusCode
            );
        }
        
        [Then(@"o header deve possuir a chave Location para o cliente registrado")]
        public void EntaoOHeaderDevePossuirAChaveLocationParaOClienteRegistrado()
        {
            var httpResponseMessage = _scenarioContext.Get<HttpResponseMessage>(REGISTER_NEW_CUSTOMER_RESPONSE_KEY);
            var locationUri = httpResponseMessage.Headers.Location;

            locationUri.Should().NotBeNull();
        }
    
        [Then(@"a response da request de registro do cliente deve condizer com o payload")]
        public void EntaoAReponseDaRequestDeRegistroDoClienteDeveCondizerComOPayload()
        {
            // Arrange
            var registerNewCustomerResponse = _scenarioContext.Get<RegisterNewCustomerResponse>(REGISTER_NEW_CUSTOMER_RESPONSE_OBJECT_KEY);
            var registerNewCustomerPayload = _scenarioContext.Get<RegisterNewCustomerPayload>(REGISTER_NEW_CUSTOMER_PAYLOAD_KEY);

            RestApiClientFixture.Compare(
                registerNewCustomerPayload,
                registerNewCustomerResponse
            ).Should().BeTrue();
        }
    
        [Then(@"o cliente retornado na Location URI deve ser o mesmo que foi registrado")]
        public async Task EntaoOClienteRetornadoNaLocationURIDeveSerOMesmoQueFoiRegistrado()
        {
            var httpResponseMessage = _scenarioContext.Get<HttpResponseMessage>(REGISTER_NEW_CUSTOMER_RESPONSE_KEY);
            var registerNewCustomerResponse = _scenarioContext.Get<RegisterNewCustomerResponse>(REGISTER_NEW_CUSTOMER_RESPONSE_OBJECT_KEY);
            
            var locationUri = httpResponseMessage.Headers.Location;
            var getCustomerByUriResponse = await _customerService.GetCustomerByUriAsync(locationUri.OriginalString, _cts.Token);

            RestApiClientFixture.Compare(
                registerNewCustomerResponse,
                getCustomerByUriResponse
            ).Should().BeTrue();
        }
    
        [Then(@"a response não deve conter mensagens de retorno")]
        public void EntaoAReponseNaoDeveConterMensagensDeRetorno()
        {
            var registerNewCustomerResponse = _scenarioContext.Get<RegisterNewCustomerResponse>(REGISTER_NEW_CUSTOMER_RESPONSE_OBJECT_KEY);
            
            registerNewCustomerResponse.MessageCollection.Should().BeNull();
        }
    }
}
