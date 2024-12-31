using MCIO.Mentorship.RestWebApi.Payloads;
using MCIO.Mentorship.RestWebApi.Responses;
using MCIO.Mentorship.RestWebApi.ViewModels;

var builder = WebApplication.CreateBuilder(args);
var customerViewModelCollection = new List<CustomerViewModel>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost("/api/customers", async (HttpRequest request, CancellationToken cancellationToken) =>
{
    var payload = await request.ReadFromJsonAsync<RegisterNewCustomerPayload>();

    var customerViewModel = new CustomerViewModel
    {
        Id = Guid.CreateVersion7(),
        FirstName = payload.FirstName,
        LastName = payload.LastName,
        BirthDate = payload.BirthDate,
        Email = payload.Email
    };

    customerViewModelCollection.Add(customerViewModel);

    return Results.Created(
        uri: $"/api/customers/{customerViewModel.Id}",
        value: new RegisterNewCustomerResponse {
            Data = customerViewModel
        }
    );
})
.WithName("PostCustomers");

app.MapGet("/api/customers/{id:guid}", (Guid id) =>
{
    var customer = customerViewModelCollection.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new GetCustomerByIdResponse{Data = customer});
})
.WithName("GetCustomers");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
