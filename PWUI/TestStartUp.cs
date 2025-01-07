using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using PWUI.Models;
using PWUI.Services;
using PWUI.Settings;
using Reqnroll.Autofac;
using Reqnroll.Infrastructure;
using ContainerBuilder = Autofac.ContainerBuilder;

namespace PWUI
{
    public static class TestStartUp
    {
        [ScenarioDependencies]
        public static void CreateServices(ContainerBuilder builder)
        {
            builder.RegisterConfiguration();
            builder.RegisterPlaywright();
            builder.RegisterAppSettings();
            builder.RegisterPages();
            builder.RegisterPageHandler();
            builder.RegisterPageDependencyService();
            builder.RegisterSteps();
        }

        private static void RegisterSteps(this ContainerBuilder builder)
        {
            builder.RegisterType<StepDefinitions>().AsSelf().InstancePerDependency();
        }

        private static void RegisterConfiguration(this ContainerBuilder builder)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Test";
            Console.WriteLine($"Current Enviornment: {env}");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"Settings/AppSettings.{env}.json", false, true)
                .Build();

            builder.RegisterInstance(configuration)
                .As<IConfiguration>()
                .SingleInstance();
        }

        private static void RegisterAppSettings(this  ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var configuration = c.Resolve<IConfiguration>();
                var appSettings = new AppSettings();
                configuration.Bind(appSettings);
                return Options.Create(appSettings);
            }).As<IOptions<AppSettings>>();
        }

        private static void RegisterPlaywright(this ContainerBuilder builder)
        {
            builder.Register(async c =>
            {
                var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
                var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    SlowMo = 200,
                }).ConfigureAwait(false);
                return await browser.NewPageAsync().ConfigureAwait(false);
            }).As<Task<IPage>>().InstancePerDependency();
        }

        private static void RegisterPages(this ContainerBuilder builder)
        {
            builder.RegisterType<LandingPage>().AsSelf().InstancePerDependency();
        }

        private static void RegisterPageHandler(this ContainerBuilder builder)
        {
            builder.RegisterType<PagesService>().As<IPageService>().InstancePerLifetimeScope();
        }

        private static void RegisterPageDependencyService(this ContainerBuilder builder)
        {
            builder.RegisterType<PageDependencyService>()
                .As<IPageDependencyService>()
                .InstancePerLifetimeScope();
        }
    }
}
