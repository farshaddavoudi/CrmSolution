﻿using Acr.UserDialogs;
using Autofac;
using Bit;
using Bit.Core.Contracts;
using Bit.Core.Implementations;
using Bit.View;
using CrmSolution.Client.MobileApp.View;
using CrmSolution.Client.MobileApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism;
using Prism.Ioc;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrmSolution.Client.MobileApp
{
    public partial class App
    {
        public static new App Current
        {
            get { return (App)Application.Current; }
        }

        public App()
            : this(null)
        {
            BitCSharpClientControls.XamlInit();
            BitApplication.XamlInit();
        }

        public App(IPlatformInitializer platformInitializer)
            : base(platformInitializer)
        {
        }

        protected override async Task OnInitializedAsync()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("/Nav/Customers");

            await base.OnInitializedAsync();
        }

        protected override void RegisterTypes(IDependencyManager dependencyManager, IContainerRegistry containerRegistry, ContainerBuilder containerBuilder, IServiceCollection services)
        {
            containerRegistry.RegisterForNav<NavigationPage>("Nav");
            containerRegistry.RegisterForNav<CustomerFormView, CustomerFormViewModel>("CustomerForm");
            containerRegistry.RegisterForNav<CustomersView, CustomersViewModel>("Customers");
            containerBuilder.Register<IClientAppProfile>(c => new DefaultClientAppProfile
            {
                AppName = "CrmSolution",
                HostUri = new Uri("http://10.0.2.2:5000/"), // ipconfig
                ODataRoute = "odata/CrmSolutionV1/"
            }).SingleInstance();

            dependencyManager.RegisterRequiredServices();
            dependencyManager.RegisterIdentityClient();
            dependencyManager.RegisterHttpClient();
            dependencyManager.RegisterODataClient();

            containerBuilder.RegisterInstance(UserDialogs.Instance);

#if DEBUG
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
#endif

            base.RegisterTypes(dependencyManager, containerRegistry, containerBuilder, services);
        }
    }
}
