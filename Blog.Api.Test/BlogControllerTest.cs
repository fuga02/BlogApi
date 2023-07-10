using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
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
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/blogs");

        //act
        var response = await _httpClient.SendAsync(request);

        //assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetBlogsTest()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/Blogs");

        //act
        var response = await _httpClient.SendAsync(request);

        //assert
        Assert.True(!response.IsSuccessStatusCode);
    }

}
