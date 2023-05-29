using aspnetcore.ntier.DAL.DataContext;
using aspnetcore.ntier.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Net;
using Xunit;

namespace aspnetcore.ntier.Test.IntegrationTests.API;

public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    private const string baseURL = "https://localhost:44338/";

    public UserControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_WhenAuthTokenIsNotProvided_ReturnsSuccess()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AspNetCoreNTierDbContext>();
            db.Database.EnsureCreated();
        }

        // Act
        var response = await _client.GetAsync("api/v1/User/getusers");
        var users = JArray.Parse(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(users);
    }
}
