using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Webscrapping.Models;
using Webscrapping.Services.Interfaces;
using OpenQA.Selenium.Interactions;

namespace Webscrapping.Services;

public class LinkedInSearch : ISearch
{
    public async Task<List<JobListing>> Search(string keyword)
    {
        var jobListings = new List<JobListing>();
        var startIndex = 0;
        int limit = 20;
        int delay = 2000;

        var options = new ChromeOptions();
        options.PageLoadStrategy = PageLoadStrategy.None;
        options.AddArguments("--headless");

        IWebDriver driver = new ChromeDriver(options);

        driver.Navigate().GoToUrl($"https://www.linkedin.com/jobs/search?keywords={keyword}&location=Brasil&f_TPR=r604800");

        await Task.Delay(delay * 4);

        Actions actions = new Actions(driver);

        try
        {
            int i = 0;
            
            while (true)
            {
                actions.SendKeys(Keys.End).Perform();
                await Task.Delay(delay);

                var newJobListings = ExtractJobListings(driver, startIndex);
                startIndex = jobListings.Count;

                var distinctNewListings = newJobListings.Except(jobListings, new JobListingComparer()).ToList();

                jobListings.AddRange(distinctNewListings);

                try
                {
                    if (distinctNewListings.Count == 0)
                    {
                        var loadMoreButton = driver.FindElement(By.XPath("/html/body/div[1]/div/main/section[2]/button"));
                        if (loadMoreButton != null)
                        {
                            loadMoreButton.Click();
                            await Task.Delay(delay);
                        }
                        else
                        {
                            break;
                        }
                    }
                } catch(Exception ex)
                {
                    break;
                }

                if (i == limit) break;

                i++;
            }
        }
        catch (Exception ex)
        {
            
        }
        finally
        {
            driver.Quit();
        }

        Console.Write(jobListings.Count);
        return jobListings;
    }

    private List<JobListing> ExtractJobListings(IWebDriver driver, int startIndex)
    {
        var jobListings = new List<JobListing>();

        IList<IWebElement> xPath = driver.FindElements(By.XPath("/html/body/div[1]/div/main/section[2]/ul/li"));

        for (int i = startIndex; i < xPath.Count; i++)
        {
            string title = xPath[i].FindElement(By.XPath(".//h3[contains(@class, 'base-search-card__title')]")).Text;
            string company = xPath[i].FindElement(By.XPath(".//h4[contains(@class, 'base-search-card__subtitle')]")).Text;
            string location = xPath[i].FindElement(By.XPath(".//span[contains(@class, 'job-search-card__location')]")).Text;
            string postedData = xPath[i].FindElement(By.XPath(".//time[contains(@class, 'job-search-card__listdate')]")).Text;
            string applyUrl = xPath[i].FindElement(By.XPath("./div/a")).GetAttribute("href");

            jobListings.Add(new JobListing
            (
                title,
                company,
                location,
                "",
                postedData,
                applyUrl
            ));
        }

        return jobListings;
    }
}

public class JobListingComparer : IEqualityComparer<JobListing>
{
    public bool Equals(JobListing x, JobListing y)
    {
        if (x == null && y == null)
            return true;
        if (x == null || y == null)
            return false;

        return x.Title == y.Title && x.Company == y.Company && x.Location == y.Location;
    }

    public int GetHashCode(JobListing obj)
    {
        return obj.Title.GetHashCode() ^ obj.Company.GetHashCode() ^ obj.Location.GetHashCode();
    }
}
