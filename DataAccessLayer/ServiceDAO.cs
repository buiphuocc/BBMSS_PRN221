using BusinessObjects;
using DataAccessLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ServiceDAO
    {
        public static List<Service> GetServices()
        {
            var services = new List<Service>();
            try
            {
                using var dbContext = new BBMSDbContext();
                services = dbContext.Services.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return services;
        }

        public static Service? GetServiceById(int id)
        {
            using var dbContext = new BBMSDbContext();
            return dbContext.Services.FirstOrDefault(b => b.Id == id);
        }

        public static bool CreateService(Service service)
        {
            try
            {
                using var dbContext = new BBMSDbContext();
                dbContext.Add(service);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool UpdateService(Service service)
        {
            try
            {
                using var dbContext = new BBMSDbContext();
                dbContext.Entry(service).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
