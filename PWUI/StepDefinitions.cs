using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PWUI.Models;
using PWUI.Services;
using Reqnroll;

namespace PWUI
{
    [Binding]
    public class StepDefinitions
    {
        private readonly IPageService _pageService;
        private readonly IPageDependencyService _pageDependencyService;
        private IBrowser _browser;
        private IPage _page;
        private static readonly Random _random = new Random();
        private static readonly Guid _guid = new Guid();

        public StepDefinitions(IPageService pageService, IPageDependencyService pageDependencyService)
        {
            _pageService = pageService ?? throw new ArgumentNullException(nameof(pageService));
            _pageDependencyService = pageDependencyService ?? throw new ArgumentNullException(nameof(pageDependencyService));
        }

        [BeforeScenario]
        public async Task SetUp()
        {
            string browserName = "chromium";
            var tags = ScenarioContext.Current.ScenarioInfo.Tags;
            var browserTag = tags.FirstOrDefault(tag => tag.StartsWith("browser-"));
            if (browserTag != null)
            {
                browserName = browserTag.Split('-')[1].ToLower();
            }

            _browser = await InitializeBrowser(browserName);
            _page = await _browser.NewPageAsync();
            _pageDependencyService.SetPage(_page);
        }

        private async Task<IBrowser> InitializeBrowser(string browserName)
        {
            var playwright = await Playwright.CreateAsync();
            bool isHeadless = true;
            return browserName.ToLower() switch
            {
                "chromium" => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = isHeadless }),
                "firefox" => await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = isHeadless }),
                "webkit" => await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = isHeadless }),
                _ => throw new ArgumentException($"Browser '{browserName}' is not supported"),
            };
        }

        private string GenerateRandomName()
        {
            var randomName = _random;
            var names = new[] { "Josh", "Steve", "Alicia", "Nala", "Shadow" };
            int index = randomName.Next(names.Length);
            return names[index];
        }

        [Given("I have navigated to the test url")]
        public async Task GivenIHaveNavigatedToTheTestUrl()
        {
            var landingPage = _pageService.LandingPage;
            await landingPage.GoToLandingPage();
        }

        [When("I have completed the Contact Form")]
        public async Task CompleteContactForm()
        {
            var landingPage = new LandingPage(_pageDependencyService);
            int randomNumber = _random.Next(1, 500); 
            string name = GenerateRandomName();
            string email = $"test{randomNumber}@example.com";

            long phoneNumber = _random.Next(100000000, 200000000) + 10000000000; 
            string subject = "Inquiry";
            string message = "A general Inquiry";
            
            await _pageService.LandingPage.SubmitContactForm(name, email, phoneNumber, subject, message);
            await landingPage.RetryContactFormSubmission(name, email, phoneNumber, subject, message);
        }
    }
}
