using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.Helpers
{
    //public class ApiHelper : IApiHelper
    //{
    //    public HttpClient apiClient { get; set; }

    //    public HttpClient ApiClient
    //    {
    //        get
    //        {
    //            return apiClient;
    //        }
    //    }

    //    public ApiHelper()
    //    {
    //        InitializeClient();

    //        var r = ConfigurationManager.AppSettings["api"];
    //    }

    //    private void InitializeClient()
    //    {
    //        apiClient = new HttpClient
    //        {
    //            BaseAddress = new Uri(ConfigurationManager.AppSettings["api"])
    //        };
    //        apiClient.DefaultRequestHeaders.Accept.Clear();
    //        apiClient.DefaultRequestHeaders.Accept.Add(
    //            new MediaTypeWithQualityHeaderValue("application/json"));
    //    }

    //    public async Task<AuthenticatedUser> Authenticate(string username, string password)
    //    {
    //        var data = new FormUrlEncodedContent(new[]
    //        {
    //            new KeyValuePair<string, string>("grant_type", "password"),
    //            new KeyValuePair<string, string>("username", username),
    //            new KeyValuePair<string, string>("password", password),
    //        });

    //        AuthenticatedUser user = null;

    //        using (HttpResponseMessage response = await apiClient.PostAsync("/Token", data))
    //        {
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var result = await response.Content.ReadAsStringAsync();
    //                user = JsonConvert.DeserializeObject<AuthenticatedUser>(result);


    //            }
    //            else
    //            {
    //                throw new Exception(response.ReasonPhrase);
    //            }
    //        }

    //        return user;
    //    }
    //}
}
