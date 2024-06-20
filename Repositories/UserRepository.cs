using BusinessObjects;
using DataAccessLayer;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public bool CreateUser(User user)
            => UserDAO.CreateUser(user);

        public User? GetUserById(int id)
            => UserDAO.GetUserById(id);

        public List<User> GetUsers()
            => UserDAO.GetUsers();

        public bool UpdateUser(User user)
            => UserDAO.UpdateUser(user);
    }
}
