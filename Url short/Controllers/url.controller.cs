using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Url;

namespace UrlController;

[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    private readonly IUrl _urlservice;
    public UrlController(IUrl url)
    {
        _urlservice = url;
    }


    [HttpGet("{url}")]

    public async Task<ActionResult> getUrl(string url)
    {
        var result = await _urlservice.getUrl(url);

        if (result != null)
        {
            return Ok(result);
        }

        return BadRequest("Not able to generate");
    }


    [HttpGet("{shorturl}/stats")]

    public async Task<ActionResult<UrlStatsRes>> getStats(string shorturl)
    {
        var result = await _urlservice.getStats(shorturl);

        if (result != null)
        {
            return Ok(result);
        }

        return BadRequest("Not able to generate");
    }



    [HttpPost]
    public async Task<ActionResult> CreateShortUrl([FromBody]UrlReq url)
    {
        var result = await _urlservice.createUrl(url.url);

        if (result != null)
        {
            return StatusCode(StatusCodes.Status201Created, result);
        }

        return BadRequest("Not able to generate");
    }

    [HttpPut("{shortcode}")]
    public async Task<ActionResult> UpdateUrl([FromBody]UrlReq url,string shortcode)
    {
        var result = await _urlservice.updateUrl(shortcode,url.url);

        if (result != null)
        {
            return Ok(result);
        }

        return BadRequest("Not able to generate");
    }

     [HttpDelete("{shortcode}")]
    public async Task<ActionResult> DeleteUrl(string shortcode)
    {
        var result = await _urlservice.deleteUrl(shortcode);

        if (result)
        {
            return Ok("Deleted successfully");
        }

        return BadRequest("Not able to generate");
    }


}