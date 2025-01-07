using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PWUI.Services;

namespace PWUI.Models
{
    public class LandingPage
    {
        private readonly IPageDependencyService _pageDependencyService;

        public LandingPage(IPageDependencyService pageDependencyService)
        {
            _pageDependencyService = pageDependencyService ?? throw new ArgumentNullException(nameof(pageDependencyService));
        }

        public async Task GoToLandingPage()
        {
            await _pageDependencyService.Page.Result.GotoAsync(_pageDependencyService.AppSettings.Value.TestUrl);
        }

        private async Task<ILocator> GetNameField()
        {
            var page = await _pageDependencyService.Page;
            return page.Locator("//*[@id='name']");
        }

        private async Task<ILocator> GetEmailField()
        {
            var page = await _pageDependencyService.Page;
            return page.Locator("//*[@id='email']");
        }

        private async Task<ILocator> GetPhoneNumberField()
        {
            var page = await _pageDependencyService.Page;
            return page.Locator("//*[@id='phone']");
        }

        private async Task<ILocator> GetSubjectField()
        {
            var page = await _pageDependencyService.Page;
            return page.Locator("//*[@id='subject']");
        }

        private async Task<ILocator> GetMessageField()
        {
            var page = await _pageDependencyService.Page;
            return page.Locator("//*[@id='description']");
        }

        private async Task<ILocator> GetSubmitButton()
        {
            var page = await _pageDependencyService.Page;
            return page.Locator("//*[@id='submitContact']");
        }

        private async Task<ILocator> GetErrorMessage()
        {
            var page = await _pageDependencyService.Page;
            return page.Locator("//*[@class='alert alert-danger']");
        }

        private async Task<bool>IsSuccessMessageDisplayed()
        {
            var page = await _pageDependencyService.Page;
            var successMessageLocator = page.Locator("//*[@class='col-sm-5'][contains(.,'Thanks for getting in touch')][1]");
            return await successMessageLocator.IsVisibleAsync();
        }

        public async Task<bool> SubmitContactForm(string name, string email, long phoneNumber, string subject, string message)
        {
            var nameField = await GetNameField();
            var emailField = await GetEmailField();
            var phoneField = await GetPhoneNumberField();
            var subjectField = await GetSubjectField();
            var messageField = await GetMessageField();
            var submitButton = await GetSubmitButton();
            var errorMessage = await GetErrorMessage();

            await nameField.FillAsync(name);
            await emailField.FillAsync(email);
            await phoneField.FillAsync(phoneNumber.ToString()); 
            await subjectField.FillAsync(subject);
            await messageField.FillAsync(message);

            await submitButton.ClickAsync();
            await Task.Delay(1000);

            if (await IsSuccessMessageDisplayed())
            {
                return false; 
            }

            return await errorMessage.IsVisibleAsync(); 
        }

        public async Task RetryContactFormSubmission(string name, string email, long phoneNumber, string subject, string message)
        {
            bool formSubmittedSuccessfully = false;
            int maxRetries = 5; 
            int retryCount = 0;

            while (!formSubmittedSuccessfully && retryCount < maxRetries)
            {
                var hasError = await SubmitContactForm(name, email, phoneNumber, subject, message);

                if (hasError)
                {
                    subject += "XXXXXXX"; 
                    message += "XXXXXX"; 
                    retryCount++;
                }
                else
                {
                    formSubmittedSuccessfully = true;
                }
            }

            if (!formSubmittedSuccessfully)
            {
                throw new Exception("Form submission failed after maximum retries.");
            }
        }
    }
}
