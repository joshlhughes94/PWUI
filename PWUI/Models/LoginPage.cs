using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucumberExpressions.Ast;
using Microsoft.Playwright;
using PWUI.Services;

namespace PWUI.Models
{
    public class LoginPage
    {
        private readonly IPageDependencyService _pageDependencyService;
        private ILocator _usernameField => _pageDependencyService.Page.Result.Locator("#user-name");
        private ILocator _passwordField => _pageDependencyService.Page.Result.Locator("#password");
        private ILocator _loginButton => _pageDependencyService.Page.Result.Locator("#login-button");
        private ILocator _swagLabsPageHeading => _pageDependencyService.Page.Result.Locator("//*[@class='header_label'][1][contains(.,'Swag Labs')]");

        public LoginPage(IPageDependencyService pageDependencyService)
        {
            _pageDependencyService = pageDependencyService ?? throw new ArgumentNullException(nameof(pageDependencyService));   
        }

        public async Task GoToPageAsync()
        {
            await _pageDependencyService.Page.Result.GotoAsync(_pageDependencyService.AppSettings.Value.TestUrl);
        }

        public async Task LoginAsync(string username, string password)
        {
            await _usernameField.FillAsync(username);
            await _passwordField.FillAsync(password);
            await _loginButton.ClickAsync();
        }

        public async Task CheckLoginWasSuccessful()
        {
            var headingIsVisble = await _swagLabsPageHeading.IsVisibleAsync();

            if (headingIsVisble)
            {
                Console.WriteLine("Heading Is Visble");
            }
            else
            {
                Console.WriteLine("Heading Is Not Visble");
            }
        }
    }
}
