using SamrtCRM.Data.Models;
using SmartCRM.Core.Filters;
using System.Collections.Generic;

namespace SmartCRM.Service.Users
{
    public interface IUserService
    {
        IEnumerable<User> GetListByFilter(UserSearchFilter filter);
        User GetDetailsById(int id);
        User GetDetailsByUsername(string username);
        bool ResetPassword(User user);
        bool Add(User user);
        bool Update(User user);
        bool Remove(User contact);
    }
}
