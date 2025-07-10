using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _user;
        private readonly IApiHelper _apiHelper;
        private readonly LoginViewModel _loginViewModel;
        public bool IsLoggedIn { get; set; }

        public ShellViewModel(IEventAggregator events, 
            ILoggedInUserModel loggedInUser, IApiHelper apiHelper)
        {
            
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
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async void ExitApplication()
        {
            await TryCloseAsync();
        }

        public void UserManagement()
        {
            ActivateItemAsync(IoC.Get<UserDisplayViewModel>());
        }

        public async Task LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>());
            IsLoggedIn = false;
            NotifyOfPropertyChange(() => IsLoggedIn);
            _loginViewModel.OnLogin -= _loginViewModel_OnLogin;
            
        }
    }
}
