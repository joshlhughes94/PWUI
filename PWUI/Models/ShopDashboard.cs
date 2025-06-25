using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PWUI.Services;

namespace PWUI.Models
{
    public class ShopDashboard
    {
        private readonly IPageDependencyService _pageDependencyService;
        private ILocator _backpackAddToCartButton => _pageDependencyService.Page.Result.Locator("#add-to-cart-sauce-labs-backpack");
        private ILocator _basketIcon => _pageDependencyService.Page.Result.Locator("#shopping_cart_container");

        public ShopDashboard(IPageDependencyService pageDependencyService)
        {
            _pageDependencyService = pageDependencyService ?? throw new ArgumentNullException(nameof(pageDependencyService));
        }

        public async Task AddBackPackToCart()
        {
            await _backpackAddToCartButton.ClickAsync();
        }

        public async Task GoToBasket()
        {
            await _basketIcon.ClickAsync();
        }
    }
}
