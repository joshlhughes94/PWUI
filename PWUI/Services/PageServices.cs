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
        LandingPage LandingPage { get; }
    }

    public class PagesService : IPageService
    {
        public PagesService(LandingPage landingPage)
        {
            LandingPage = landingPage ?? throw new ArgumentNullException(nameof(landingPage));
        }

        public LandingPage LandingPage { get;}
    }
}
