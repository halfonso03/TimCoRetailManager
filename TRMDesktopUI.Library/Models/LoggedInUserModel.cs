using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        //public static ILoggedInUserModel Instance = new LoggedInUserModel();
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreateDate { get; set; }
        public string Token { get; set; }

        public void LogOffUser()
        {
            Token = "";
            FirstName = "";
            LastName = "";
            EmailAddress = string.Empty;
            Id = string.Empty;
            CreateDate = DateTime.MinValue;
        }
    }
}
