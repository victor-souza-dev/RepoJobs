using Webscrapping.Models;
namespace Webscrapping.Services.Interfaces;

public interface ISearch
{
    Task<List<JobListing>> Search(string keyword);
}