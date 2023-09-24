using Microsoft.Extensions.Caching.Memory;
using Webscrapping.Models;
using Webscrapping.Models.DTOs;
using Webscrapping.Services.Interfaces;

namespace Webscrapping.Services;

public class JobScraperService : IJobScraperService
{
    private readonly IMemoryCache _cache;
    private readonly ISearch _linkedInSearch;

    public JobScraperService(IMemoryCache cache, ISearch linkedInSearch)
    {
        _cache = cache;
        _linkedInSearch = linkedInSearch;
    }

    public async Task<List<JobListing>> ScrapeJobListings(JobSearchQueryParamsDTO queryParams)
    {
        var cacheKey = $"jobListings_{queryParams.Keyword}_{queryParams.Page}_{queryParams.PageSize}";

        if (_cache.TryGetValue(cacheKey, out List<JobListing> cachedJobListings))
        {
            return cachedJobListings;
        }
        else
        {
            var jobListings = await _linkedInSearch.Search(queryParams.Keyword);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            _cache.Set(cacheKey, jobListings, cacheEntryOptions);

            return jobListings;
        }
    }

}
