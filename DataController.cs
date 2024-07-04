using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DataController : ControllerBase
{
    private readonly HttpClient httpClient;

    public DataController(){
        httpClient = new HttpClient();
    }

    [HttpGet]
    public async Task<IActionResult> GetLyrics(){
        var apiUrl = "https://www.azlyrics.com/lyrics/queen/wewillrockyou.html";

        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode){
            string data = await response.Content.ReadAsStringAsync();
            
            return Ok(data);
        }
        else{
            return StatusCode((int)response.StatusCode);
        }
    }
}
