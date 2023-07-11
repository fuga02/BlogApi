using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Blog.Api.Test;

public class BlogControllerTest
{
    private readonly HttpClient _httpClient;

    public BlogControllerTest()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        _httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task IsAuthorized()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:3030/api/Blogs");

        //act
        var response = await _httpClient.SendAsync(request);

        //assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetBlogsTest()
    {
        var token =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjJhZmZlMmMwLWM0OTYtNDU4Zi1iYzA2LWY3OTRhOGUwOTYyNCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJzdHJpbmciLCJleHAiOjE2ODkxNDg1OTQsImlzcyI6IkJsb2cuQXBpIiwiYXVkIjoiQmxvZy5TZXJ2aWNlcyJ9.9PIL0ooFeJZcqEYZWMJOIRzo9M2yIdmXmLnrQBMqOWY";
        var request = new HttpRequestMessage(HttpMethod.Get, "api/Blogs");
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await _httpClient.SendAsync(request);

        //assert
        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
    }
    

}
