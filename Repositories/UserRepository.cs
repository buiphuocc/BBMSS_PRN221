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
        private readonly UserDAO userDAO;

        public UserRepository(UserDAO userDAO)
        {
            this.userDAO = userDAO;
        }
        public void AddUser(User user)
            => userDAO.AddUser(user);

        public void DeleteUser(int id)
            => userDAO.DeleteUser(id);

        public List<User> GetAllUsers()
            => userDAO.GetAllUsers();

        public User GetUserById(int id)
            => userDAO.GetUserById(id);

        public User? GetUserByUserName(string userName)
            => userDAO.GetUserByUserName(userName);

        public void UpdateUser(User user)
            => userDAO.UpdateUser(user);
    }
}
