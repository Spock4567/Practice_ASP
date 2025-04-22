using Practice_ASP;
using Practice_ASP.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.HttpResults;

namespace RequestLimiterTestProject
{
    public class UnitTest1
    {
        [Fact]
        public async Task Error503Test()
        {
            int limit = 5;

            var mockProcessor = new Mock<StringProcessorService>(
       Mock.Of<HttpClient>(),
       Mock.Of<ILogger<StringProcessorService>>(),
       Mock.Of<IOptions<AppConfig>>());

            var controller = new StringProcessingController(
        mockProcessor.Object,
        new RequestLimiterService(limit));

            var results = new List<IActionResult>();
            for (int i = 0; i < limit + 1; i++)
            {
                results.Add(await controller.ProcessString("test"));
            }

            //Assert.Equal(5, results.Count(r => r is OkObjectResult)); // 5 успешных
            Assert.Equal(1, results.Count(r => r is StatusCodeResult sc && sc.StatusCode == 503)); // 5 ошибок
        }
    }
}