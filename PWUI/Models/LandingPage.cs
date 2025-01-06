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

        // Method to check for success message visibility
        private async Task<bool>IsSuccessMessageDisplayed()
        {
            var page = await _pageDependencyService.Page;
            Thread.Sleep(4000);
            var successMessageLocator = page.Locator("//*[@style='height: 412px;']");
            return await successMessageLocator.IsVisibleAsync();
        }

        // Method to submit the contact form
        public async Task<bool> SubmitContactForm(string name, string email, long phoneNumber, string subject, string message)
        {
            var nameField = await GetNameField();
            var emailField = await GetEmailField();
            var phoneField = await GetPhoneNumberField();
            var subjectField = await GetSubjectField();
            var messageField = await GetMessageField();
            var submitButton = await GetSubmitButton();
            var errorMessage = await GetErrorMessage();

            // Fill in the form fields
            await nameField.FillAsync(name);
            await emailField.FillAsync(email);
            await phoneField.FillAsync(phoneNumber.ToString()); // Fill phone field with phoneNumber as string
            await subjectField.FillAsync(subject);
            await messageField.FillAsync(message);

            // Click the submit button
            await submitButton.ClickAsync();

            // Check for success message
            if (await IsSuccessMessageDisplayed())
            {
                return false; 
            }

            // Check for error message
            return await errorMessage.IsVisibleAsync(); // Return true if an error is present
        }

        // Method to retry contact form submission in case of errors
        public async Task RetryContactFormSubmission(string name, string email, long phoneNumber, string subject, string message)
        {
            bool formSubmittedSuccessfully = false;
            int maxRetries = 13; 
            int retryCount = 0;

            while (!formSubmittedSuccessfully && retryCount < maxRetries)
            {
                var hasError = await SubmitContactForm(name, email, phoneNumber, subject, message);

                if (hasError)
                {
                    // Update subject and message with additional characters
                    subject += "XXXXXXX"; // Add characters to subject for retry
                    message += "XXXXXX";  // Add characters to message for retry
                    retryCount++;
                }
                else
                {
                    formSubmittedSuccessfully = true; // Form submitted successfully
                }
            }

            if (!formSubmittedSuccessfully)
            {
                throw new Exception("Form submission failed after maximum retries.");
            }
        }

    }
}
