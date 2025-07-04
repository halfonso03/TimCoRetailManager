using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SalesViewModel _saleVM;
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _user;
        private readonly IApiHelper _apiHelper;
        private readonly LoginViewModel _loginViewModel;
        public bool IsLoggedIn { get; set; }

        public ShellViewModel(SalesViewModel saleVM, IEventAggregator events, 
            ILoggedInUserModel loggedInUser, IApiHelper apiHelper)
        {
            _saleVM = saleVM;
            
            _events = events;
            _user = loggedInUser;
            _apiHelper = apiHelper;
            _events.SubscribeOnPublishedThread(this);            
            
            _loginViewModel = IoC.Get<LoginViewModel>();

            ActivateItemAsync(_loginViewModel);

            _loginViewModel.OnLogin += _loginViewModel_OnLogin;

        }

        private void _loginViewModel_OnLogin()
        {
            IsLoggedIn = true;
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_saleVM);
        }

        public async void ExitApplication()
        {
            await TryCloseAsync();
        }

        public void LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();
            IsLoggedIn = false;
            NotifyOfPropertyChange(() => IsLoggedIn);
            _loginViewModel.OnLogin -= _loginViewModel_OnLogin;
            ActivateItemAsync(_loginViewModel);
        }
    }
}
