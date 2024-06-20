using BusinessObjects;
using DataAccessLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CourtDAO
    {
        public static List<Court> GetCourts()
        {
            var courts = new List<Court>();
            try
            {
                using var dbContext = new BBMSDbContext();
                courts = dbContext.Courts.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return courts;
        }

        public static Court? GetCourtById(int id)
        {
            using var dbContext = new BBMSDbContext();
            return dbContext.Courts.FirstOrDefault(b => b.Id == id);
        }

        public static bool CreateCourt(Court court)
        {
            try
            {
                using var dbContext = new BBMSDbContext();
                dbContext.Add(court);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool UpdateCourt(Court court)
        {
            try
            {
                using var dbContext = new BBMSDbContext();
                dbContext.Entry(court).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
