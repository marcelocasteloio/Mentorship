using System;
using IntegrationTests.Fixtures;
using TechTalk.SpecFlow.Assist.Attributes;
using Xunit;

namespace IntegrationTests.Models;

public class RegisterNewCustomerTable
    : IClassFixture<RestApiClientFixture>
{
    [TableAliases("first-name")]
    public string FirstName { get; set; }

    [TableAliases("last-name")]
    public string LastName { get; set; }

    [TableAliases("birth-date")]
    public string BirthDate { get; set; }

    [TableAliases("email")]
    public string Email { get; set; }
}
