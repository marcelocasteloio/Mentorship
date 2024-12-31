using System;
using IntegrationTests.Models;
using IntegrationTests.Models.Payloads;

namespace IntegrationTests.Factories;

public static class RegisterNewCustomerPayloadFactory
{
    public static RegisterNewCustomerPayload Create(RegisterNewCustomerTable registerNewCustomerTableItem)
    {
        return new RegisterNewCustomerPayload
        {
            FirstName = registerNewCustomerTableItem.FirstName,
            LastName = registerNewCustomerTableItem.LastName,
            BirthDate = registerNewCustomerTableItem.BirthDate,
            Email = registerNewCustomerTableItem.Email
        };
    }
}
