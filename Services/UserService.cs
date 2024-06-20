using BusinessObjects;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public bool CreateUser(User user)
        {
            return userRepository.CreateUser(user);
        }

        public User? GetUserById(int id)
        {
            return userRepository?.GetUserById(id);
        }

        public List<User> GetUsers()
        {
            return userRepository.GetUsers();
        }

        public bool UpdateUser(User user)
        {
            return userRepository.UpdateUser(user);
        }
    }
}
