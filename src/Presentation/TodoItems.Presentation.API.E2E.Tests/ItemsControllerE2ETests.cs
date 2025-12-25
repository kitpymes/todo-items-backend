using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;

namespace TodoItems.Presentation.API.E2E.Tests;

public class ItemsControllerE2ETests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateItem_InvalidModel_ShouldReturnBadRequest()
    {
        // Arrange
        var payload = new
        {
            Title = "",
            Description = "End to End test",
            Category = "Architecture"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/Item/Add", payload);

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest, $"Esperaba: {HttpStatusCode.BadRequest} | Devolvió: {response.StatusCode}");
    }

    [Fact]
    public async Task CreateItem_ShouldCreateItem()
    {
        // Arrange
        var payload = new
        {
            Title = "E2E DDD",
            Description = "End to End test",
            Category = "Architecture"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/Item/Add", payload);
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"Esperaba: {HttpStatusCode.OK} | Devolvió: {response.StatusCode}");
    }

    [Fact]
    public async Task FullFlow_ShouldCreateItem_And_RegisterProgress()
    {
        // Arrange
        var payload = new
        {
            Title = "E2E DDD",
            Description = "End to End test",
            Category = "Architecture"
        };

        // Act

        var response = await _client.PostAsJsonAsync("/Item/Add", payload);
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"Esperaba: {HttpStatusCode.OK} | Devolvió: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();

        dynamic json = JObject.Parse(content);

        var newItemId = json.data.id.ToString();

        // Act
        var progressPayload = new
        {
            ItemId = new Guid(newItemId),
            Percent = 50
        };

        var progressResponse = await _client.PostAsJsonAsync("/Item/RegisterProgress", progressPayload);
        progressResponse.EnsureSuccessStatusCode();

        // Assert
        Assert.True(progressResponse.IsSuccessStatusCode, $"Esperaba: {HttpStatusCode.OK} | Devolvió: {progressResponse.StatusCode}");
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_AndContainCreatedItem()
    {
        // Arrange
        var title = Guid.NewGuid().ToString("N")[..8];
        var payload = new
        {
            Title = title,
            Description = "End to End test",
            Category = "Architecture"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/Item/Add", payload);
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"Esperaba: {HttpStatusCode.OK} | Devolvió: {response.StatusCode}");


        // Act
        var getResponse = await _client.GetAsync("/Item/GetAll");
        getResponse.EnsureSuccessStatusCode();

        // Assert
        var content = await getResponse.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(content));
        Assert.Contains(title, content);
    }    
}
