namespace Webscrapping.Models;

public class JobListing
{
    public string Title { get; private set; }
    public string Company { get; private set; }
    public string Location { get; private set; }
    public string Description { get; private set; }
    public string PostedDate { get; private set; }
    public string ApplyUrl { get; private set; }

    public JobListing(string title, string company, string location, string description, string postedDate, string applyUrl)
    {
        Title = title;
        Company = company;
        Location = location;
        Description = description;
        PostedDate = postedDate;
        ApplyUrl = applyUrl;
    }
}
