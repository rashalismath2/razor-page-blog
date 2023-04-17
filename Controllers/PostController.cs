using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using System.Text;

namespace BlogSite.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient http;
        private readonly string aiEndpoint;

        public PostController(AppDbContext dbContex, IConfiguration configs)
        {
            _dbContext = dbContex;
            this.http = new HttpClient();
            aiEndpoint = new Uri(configs["Audio_Endpoint"]).ToString();
        }
        [HttpGet("{id}/audiobook")]
        public async Task<ActionResult<string>> GetAudioBook([FromRoute] int id)
        {
 
            var post = await _dbContext.Posts.Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            var jsonString = JsonConvert.SerializeObject(new AIEndpointRequestBody(post.Title, post.Body));
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await http.PostAsync(aiEndpoint, stringContent);
            var httpResonse = "";
            if (response.IsSuccessStatusCode)
            {
                httpResonse = await response.Content.ReadAsStringAsync();
            }
            return Ok(httpResonse);
        }
    }
}
