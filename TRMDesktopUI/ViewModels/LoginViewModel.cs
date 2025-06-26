using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Helpers;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
		private IApiHelper _apiHelper;
		private string _userName;

		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;

				NotifyOfPropertyChange(() => UserName);	
				NotifyOfPropertyChange(() => CanLogin);	
			}
		}


		private string _password;

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




		public LoginViewModel(ApiHelper apiHelper)
		{
			_apiHelper = apiHelper;
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

				var user = await new ApiHelper().Authenticate(UserName, Password);

			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;	
			}		
		}

	}
}
