using BusinessObjects;
using DataAccessLayer.Data;
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
        public static List<User> GetUsers()
        {
            var users = new List<User>();
            try
            {
                using var dbContext = new BBMSDbContext();
                users = dbContext.Users.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }

        public static User? GetUserById(int id)
        {
            using var dbContext = new BBMSDbContext();
            return dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public static bool CreateUser(User user)
        {
            try
            {
                using var dbContext = new BBMSDbContext();
                dbContext.Add(user);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //return false;
                throw new Exception(ex.Message);
            } 
        }

        public static bool UpdateUser(User user)
        {
            try
            {
                using var dbContext = new BBMSDbContext();
                dbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //return false;
                throw new Exception(ex.Message);
            }
        }

    }
}
