using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	public class LoginViewModel : Screen
	{
		private IApiHelper _apiHelper;
		private readonly IEventAggregator _events;
		private string _UserName = "hector.alfonso@yahoo.com";

		public string UserName
		{
			get { return _UserName; }
			set
			{
				_UserName = value;

				NotifyOfPropertyChange(() => UserName);	
				NotifyOfPropertyChange(() => CanLogin);	
			}
		}


		private string _password = "Password#1";

		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;

				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogin);

			}
		}

		public bool IsErrorVisible => !string.IsNullOrEmpty(ErrorMessage);

		private string _errorMessage;

		public string ErrorMessage
		{
			get { return _errorMessage; }
			set
			{
				_errorMessage = value;
				NotifyOfPropertyChange(() => IsErrorVisible);
				NotifyOfPropertyChange(() =>  ErrorMessage);
			}
		}

		public LoginViewModel(IApiHelper apiHelper, IEventAggregator events)
		{
			_apiHelper = apiHelper;
			_events = events;
		}

		public bool CanLogin
		{
			get
			{
				bool output = false;
				if (UserName?.Length > 0 && Password?.Length > 0)
				{
					output = true;
				}

				return output;
			}
		}

		public async void Login()
		{


			try
			{
				ErrorMessage = string.Empty;

				var apiHelper = new ApiHelper();

				AuthenticatedUser user = await apiHelper.Authenticate(UserName, Password);

				await _apiHelper.GetLoginUserInfo(user.Access_Token);

				await _events.PublishOnUIThreadAsync(new LogOnEvent());


			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;	
			}		
		}

	}
}
