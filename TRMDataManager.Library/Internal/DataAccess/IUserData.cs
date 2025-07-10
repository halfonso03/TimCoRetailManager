using System.Collections.Generic;
using TRMDataManager.Library.Internal.Models;

namespace TRMDataManager.Library.Internal.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string Id);
    }
}