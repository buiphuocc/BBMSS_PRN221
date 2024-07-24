using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserDAO
    {
        //private readonly BadmintonBookingSystemContext _context;

        //public UserDAO(BadmintonBookingSystemContext context)
        //{
        //    _context = context;
        //}

        public static List<User> GetAllUsers()
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Users.ToList();
        }

        public static User GetUserByUserName(string userName)
        {
            using var _context = new BadmintonBookingSystemContext();
            User user = _context.Users.FirstOrDefault(u=> u.Username.Equals(userName));
            return user;
        }

        public static User GetUserById(int id)
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Users.Find(id);
        }

        public static void AddUser(User user)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public static void UpdateUser(User user)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public static void DeleteUser(int id)
        {
            using var _context = new BadmintonBookingSystemContext();
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public static bool IsUserExist(string email, string username)
        {
            using var _context = new BadmintonBookingSystemContext();
            var user = _context.Users.FirstOrDefault(u => u.Email.Equals(email) || u.Username.Equals(username));
            return user != null;
        }
    }
}
