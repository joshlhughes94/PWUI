using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gherkin.Ast;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using PWUI.Settings;
using Reqnroll;

namespace PWUI.Services
{
    public interface IPageDependencyService
    {
        Task<IPage> Page { get; } 
        IOptions<AppSettings> AppSettings { get; }
        ScenarioContext ScenarioContext { get; }
        void SetPage(IPage page);
    }

    public class PageDependencyService : IPageDependencyService, IDisposable
    {
        private Task<IPage> _page;
        public PageDependencyService(Task<IPage> page, IOptions<AppSettings> appSettings, ScenarioContext scenarioContext)
        {
            SetPage(page.Result);
            AppSettings = appSettings;
            ScenarioContext = scenarioContext;
        }

        public Task<IPage> Page => _page;
        public IOptions<AppSettings> AppSettings { get; }
        public ScenarioContext ScenarioContext { get; }

        public void SetPage(IPage page)
        {
            _page = Task.FromResult(page);
        }

        public void Dispose()
        {
            if (_page != null)
            {
                _page.Result.Context.Browser?.CloseAsync();
            }
        }
    }
}
