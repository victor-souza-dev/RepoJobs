using Webscrapping.Models;
using Webscrapping.Models.DTOs;

namespace Webscrapping.Services.Interfaces;

public interface IJobScraperService
{
    Task<List<JobListing>> ScrapeJobListings(JobSearchQueryParamsDTO queryParams);
}
