using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
