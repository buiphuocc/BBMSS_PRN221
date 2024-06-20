using BusinessObjects;
using DataAccessLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetUsers();

        User? GetUserById(int id);

        bool CreateUser(User user);

        bool UpdateUser(User user);
    }
}
