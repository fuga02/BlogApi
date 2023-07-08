using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using BlogApi.Controllers;
using Microsoft.Extensions.DependencyInjection;

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
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:3030/api/Blogs");

        //act
        var response = await _httpClient.SendAsync(request);

        //assert
        Assert.True(!response.IsSuccessStatusCode);
    }

}
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var blogController = services.SingleOrDefault(c => c.ServiceType == typeof(BlogsController));

            if (blogController != null)
                services.Remove(blogController);

            /*var priceMock = new BlogsController();

            services.AddSingleton<blo>(f => priceMock);*/
        });

        builder.UseEnvironment("Development");
    }
}