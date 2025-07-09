using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TRMDesktopUI.Library.Helpers
{
    public class ApiHelper : IApiHelper
    {
        public HttpClient _httpClient { get; set; }
        private ILoggedInUserModel _loggedInUser;


        public HttpClient ApiClient
        {
            get
            {
                return _httpClient;
            }
        }


        public ApiHelper()
        {
            InitializeClient();
        }

        public ApiHelper(ILoggedInUserModel loggedInUser)
        {
            _loggedInUser = loggedInUser;
            InitializeClient();
        }

        private void InitializeClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["api"])
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            AuthenticatedUser user = null;

            using (HttpResponseMessage response = await _httpClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<AuthenticatedUser>(result);


                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

            return user;
        }


        public async Task GetLoginUserInfo(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (HttpResponseMessage response = await _httpClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LoggedInUserModel>(user);

                    _loggedInUser.CreateDate = result.CreateDate;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.Token = token;
                    _loggedInUser.Id = result.Id;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public void LogOffUser()
        {
            _httpClient.DefaultRequestHeaders.Clear();
        }
    }
}
