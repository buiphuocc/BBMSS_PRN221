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
        List<User> GetAllUsers();

        User GetUserById(int id);

        User? GetUserByUserName(string userName);

        void AddUser(User user);

        void UpdateUser(User user);

        void DeleteUser(int id);
    }
}
