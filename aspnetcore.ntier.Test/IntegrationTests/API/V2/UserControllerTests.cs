using aspnetcore.ntier.DAL.DataContext;
using aspnetcore.ntier.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace aspnetcore.ntier.Test.IntegrationTests.API.V2;

public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public UserControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_WhenAuthTokenIsNotProvided_ReturnsUnauthorized()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AspNetCoreNTierDbContext>();
            await db.Database.EnsureCreatedAsync();
        }

        // Act
        var response = await _client.GetAsync("api/v2/User/getusers");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
