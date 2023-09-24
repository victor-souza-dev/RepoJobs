using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Webscrapping.Models;
using Webscrapping.Services.Interfaces;
using OpenQA.Selenium.Interactions;
using static System.Net.Mime.MediaTypeNames;

namespace Webscrapping.Services
{
    public class LinkedInSearch : ISearch
    {
        public async Task<List<JobListing>> Search(string keyword)
        {
            IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl($"https://www.linkedin.com/jobs/search?keywords={keyword}&location=Brasil&f_TPR=r604800");

            await Task.Delay(5000);

            Actions actions = new Actions(driver);

            var jobListings = new List<JobListing>();
            var numberOfEndClicks = 0;

            try
            {
                while (numberOfEndClicks < 5)
                {
                    actions.SendKeys(Keys.End).Perform();
                    await Task.Delay(3000);


                    numberOfEndClicks++;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                var newJobListings = ExtractJobListings(driver);
                jobListings.AddRange(newJobListings);
                driver.Quit();
            }

            return jobListings;
        }


        private List<JobListing> ExtractJobListings(IWebDriver driver)
        {
            var jobListings = new List<JobListing>();

            IList<IWebElement> titleXPath = driver.FindElements(By.XPath(".//h3[contains(@class, 'base-search-card__title')]"));
            IList<IWebElement> companyXPath = driver.FindElements(By.XPath(".//h4[contains(@class, 'base-search-card__subtitle')]"));
            IList<IWebElement> locationXPath = driver.FindElements(By.XPath(".//span[contains(@class, 'job-search-card__location')]"));
            IList<IWebElement> descriptionXPath = driver.FindElements(By.XPath(".//div[contains(@class, 'job-search-card__benefits')]"));
            IList<IWebElement> postedDataXPath = driver.FindElements(By.XPath(".//time[contains(@class, 'job-search-card__listdate')]"));
            IList<IWebElement> applyUrlXPath = driver.FindElements(By.XPath(".//a[contains(@class, 'base-card__full-link absolute top-0 right-0 bottom-0 left-0 p-0 z-[2]')]"));

            for (int i = 0; i < titleXPath.Count; i++)
            {
                string title = titleXPath[i].Text;
                string company = companyXPath[i].Text;
                string location = locationXPath[i].Text;
                string postedData = postedDataXPath[i].Text;

                jobListings.Add(new JobListing
                (
                    title,
                    company,
                    location,
                    "",
                    postedData,
                    ""
                ));
            }


            return jobListings;
        }
    }
}
