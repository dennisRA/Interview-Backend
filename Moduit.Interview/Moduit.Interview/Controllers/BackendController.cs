using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moduit.Interview.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Moduit.Interview.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BackendController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BackendController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> AnswerOne()
        {
            ScreeningItemResponseModel answerResponse;
            HttpClient client = GetHttpClient();
            var httpResponse = await client.GetAsync("one");
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStreamAsync();
                answerResponse = await JsonSerializer.DeserializeAsync<ScreeningItemResponseModel>(content);
            }
            else
            {
                return NotFound();
            }

            return Ok(answerResponse);
        }

        [HttpGet]
        public async Task<IActionResult> AnswerTwo()
        {
            List<ScreeningItemResponseModel> answerResponse = new List<ScreeningItemResponseModel>();
            HttpClient client = GetHttpClient();
            var httpResponse = await client.GetAsync("two");
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStreamAsync();

                List<ScreeningItemResponseModel> data = await JsonSerializer.DeserializeAsync<List<ScreeningItemResponseModel>>(content);
                answerResponse = data.Where(x => (x.description.Contains("Ergonomic") || x.title.Contains("Ergonomic"))&& (x.tags != null && x.tags.Contains("Sports")))
                                     .OrderByDescending(x => x.id)
                                     .TakeLast(3)
                                     .ToList();
            }
            else
            {
                return NotFound();
            }

            return Ok(answerResponse);
        }

        [HttpGet]
        public async Task<IActionResult> AnswerThree()
        {
            List<ScreeningItemResponseModel> answerResponse = new List<ScreeningItemResponseModel>();
            HttpClient client = GetHttpClient();
            var httpResponse = await client.GetAsync("three");
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStreamAsync();

                List<ScreeningThirdRawResponseModel> listRawItems = await JsonSerializer.DeserializeAsync<List<ScreeningThirdRawResponseModel>>(content);
                foreach(var rawItems in listRawItems)
                {
                    if (rawItems.items != null)
                    {
                        foreach (var rawItem in rawItems.items)
                        {
                            var newItem = new ScreeningItemResponseModel()
                            {
                                id = rawItems.id,
                                category = rawItems.category,
                                createdAt = rawItems.createdAt,
                                title = rawItem.title,
                                description = rawItem.description,
                                tags = rawItems.tags,
                                footer = rawItem.footer
                            };
                            answerResponse.Add(newItem);
                        }
                    }
                    else
                    {

                        var newItem = new ScreeningItemResponseModel()
                        {
                            id = rawItems.id,
                            category = rawItems.category,
                            createdAt = rawItems.createdAt == null ? null : rawItems.createdAt,
                            tags = rawItems.tags
                        };
                        answerResponse.Add(newItem);
                    }
                }
            }
            else
            {
                return NotFound();
            }
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

                
            };

            var jsonstring = JsonSerializer.Serialize<List<ScreeningItemResponseModel>>(answerResponse);
            var json = JsonSerializer.Deserialize<List<ScreeningItemResponseModel>>(jsonstring);
            return Ok(answerResponse);
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient("screening");
            return httpClient;
        }
    }
}
