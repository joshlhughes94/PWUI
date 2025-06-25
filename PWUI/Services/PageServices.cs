using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PWUI.Models;

namespace PWUI.Services
{
    public interface IPageService
    {
        LoginPage LoginPage { get; }
        ShopDashboard ShopDashboard { get; }
        CheckoutPage CheckoutPage { get; }
    }

    public class PagesService : IPageService
    {
        public PagesService(LoginPage loginPage, ShopDashboard shopDashboard, CheckoutPage checkoutPage)
        {
            LoginPage = loginPage ?? throw new ArgumentNullException(nameof(loginPage));
            ShopDashboard = shopDashboard ?? throw new ArgumentNullException(nameof(shopDashboard));
            CheckoutPage = checkoutPage ?? throw new ArgumentNullException( nameof(checkoutPage));
        }

        public LoginPage LoginPage { get; }
        public ShopDashboard ShopDashboard { get; }
        public CheckoutPage CheckoutPage { get; }
    }
}
