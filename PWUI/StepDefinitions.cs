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

        [Given("I have navigated to the test url, logged in successfully")]
        public async Task GivenIHaveNavigatedToTheTestUrlLoggedInSuccessfully()
        {
            var username = _pageDependencyService.AppSettings.Value.Username;
            var password = _pageDependencyService.AppSettings.Value.Password;

            var loginPage = _pageService.LoginPage;
            await loginPage.GoToPageAsync();
            await loginPage.LoginAsync(username, password);
            await loginPage.CheckLoginWasSuccessful();
        }

        [Given("I add a backpack to my basket")]
        public async Task GivenIAddABackpackToMyBasket()
        {
            var shopDash = _pageService.ShopDashboard;
            await shopDash.AddBackPackToCart();
        }

        [When("I go to the basket and select Checkout")]
        public async Task WhenIGoToTheBasketAndSelectCheckout()
        {
            var shopDash = _pageService.ShopDashboard;
            var checkoutPage = _pageService.CheckoutPage;
            await shopDash.GoToBasket();
            await checkoutPage.SelectCheckoutButton();
        }

        [When("I complete the required fields to complete my checkout")]
        public async Task WhenICompleteTheRequiredFieldsToCompleteMyCheckout()
        {
            var checkoutPage = _pageService.CheckoutPage;
            await checkoutPage.FillOutCheckoutForm();
        }

        [Then("I see a message confirming that my order has been successful")]
        public async Task ThenISeeAMessageConfirmingThatMyOrderHasBeenSuccessful()
        {
            var checkoutPage = _pageService.CheckoutPage;
            await checkoutPage.SelectFinishButton();
            await checkoutPage.CheckThankYouMessageIsDisplayed();
        }
    }
}
