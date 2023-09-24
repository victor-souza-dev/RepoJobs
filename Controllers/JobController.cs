using Microsoft.AspNetCore.Mvc;
using Webscrapping.Models.DTOs;
using Webscrapping.Services.Interfaces;

namespace Webscrapping.Controllers;

[Route("[controller]")]
[ApiController]
public class JobController : Controller
{
    private readonly IJobScraperService _jobScraperService;

    public JobController(IJobScraperService jobScraperService)
    {
        _jobScraperService = jobScraperService;
    }

    [HttpGet]
    public async Task<IActionResult> SearchJobs([FromQuery] JobSearchQueryParamsDTO queryParams)
    {
        try
        {
            var jobListings = await _jobScraperService.ScrapeJobListings(queryParams);
            return Ok(jobListings);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao buscar vagas: {ex.Message}");
        }
    }
}
