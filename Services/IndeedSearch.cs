using Webscrapping.Models;
using Webscrapping.Services.Interfaces;

namespace Webscrapping.Services
{
    public class IndeedSearch : ISearch
    {
        public Task<List<JobListing>> Search(string keyword)
        {
            throw new NotImplementedException();
        }
    }
}
