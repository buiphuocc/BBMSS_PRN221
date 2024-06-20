using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        List<User> GetUsers();

        User? GetUserById(int id);

        bool CreateUser(User user);

        bool UpdateUser(User user);
    }
}
