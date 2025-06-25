using Microsoft.Playwright;
using PWUI.Services;

namespace PWUI.Models
{
    public class CheckoutPage
    {
        private readonly IPageDependencyService _pageDependencyService;
        private ILocator _checkoutButton => _pageDependencyService.Page.Result.Locator("#checkout");
        private ILocator _firstNameField => _pageDependencyService.Page.Result.Locator("#first-name");
        private ILocator _lastNameField => _pageDependencyService.Page.Result.Locator("#last-name");
        private ILocator _postCodeField => _pageDependencyService.Page.Result.Locator("#postal-code");
        private ILocator _continueButton => _pageDependencyService.Page.Result.Locator("#continue");
        private ILocator _finishButton => _pageDependencyService.Page.Result.Locator("#finish");
        private ILocator _thankYouMessageForSuccessfulOrder => _pageDependencyService.Page.Result.Locator("//*[@class='complete-header'][1][contains(.,'Thank you for your order!')]");

        public CheckoutPage(IPageDependencyService pageDependencyService)
        {
            _pageDependencyService = pageDependencyService ?? throw new ArgumentNullException(nameof(pageDependencyService));
        }

        public async Task SelectCheckoutButton()
        {
            await _checkoutButton.ClickAsync();
        }

        public async Task FillOutCheckoutForm()
        {
            Random random = new Random();
            int number = random.Next(0, 100);
            await _firstNameField.FillAsync("Test");
            await _lastNameField.FillAsync("Testing");
            await _postCodeField.FillAsync($"{number}");
            await _continueButton.ClickAsync();
        }

        public async Task SelectFinishButton()
        {
            await _finishButton.ClickAsync();
        }

        public async Task CheckThankYouMessageIsDisplayed()
        {
            var thankYouMessageIsVisible = await _thankYouMessageForSuccessfulOrder.IsVisibleAsync();
            if (thankYouMessageIsVisible)
            {
                Console.WriteLine("Order has gone through successfully");
            }
            else
            {
                Console.WriteLine("Order has not gone through");
            }
        }
    }
}
