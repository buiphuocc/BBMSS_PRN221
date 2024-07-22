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
        //private readonly UserDAO userDAO;

        //public UserRepository(UserDAO userDAO)
        //{
        //    this.userDAO = userDAO;
        //}
        public void AddUser(User user)
            => UserDAO.AddUser(user);

        public void DeleteUser(int id)
            => UserDAO.DeleteUser(id);

        public List<User> GetAllUsers()
            => UserDAO.GetAllUsers();

        public User GetUserById(int id)
            => UserDAO.GetUserById(id);

        public User? GetUserByUserName(string userName)
            => UserDAO.GetUserByUserName(userName);

        public void UpdateUser(User user)
            => UserDAO.UpdateUser(user);

        public bool IsUserExist(string email, string username)
            => UserDAO.IsUserExist(email, username);
    }
}
