﻿using BusinessObjects;
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

        public void AddUser(User user)
        {
            userRepository.AddUser(user);
        }

        public void DeleteUser(int id)
        {
            userRepository.DeleteUser(id);  
        }

        public List<User> GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }

        public User GetUserById(int id)
        {
            return (userRepository.GetUserById(id));
        }

        public User? GetUserByUserName(string userName)
        {
            return userRepository.GetUserByUserName(userName);
        }

        public void UpdateUser(User user)
        {
            userRepository.UpdateUser(user);
        }

        public bool IsUserExist(string email, string username)
        {
            return userRepository.IsUserExist(email, username);
        }
    }
}
