using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ServiceDAO
    {
        //private readonly BadmintonBookingSystemContext _context;

        //public ServiceDAO(BadmintonBookingSystemContext context)
        //{
        //    _context = context;
        //}

        public static List<Service> GetAllServices()
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Services.ToList();
        }

        public static List<Service> GetAllActiveServices()
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Services.Where(s => s.IsActive == true).ToList();
        }

        public static Service GetServiceById(int id)
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Services.Find(id);
        }

        public static void AddService(Service service)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public static void UpdateService(Service service)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Entry(service).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public static void DeleteService(int id)
        {
            using var _context = new BadmintonBookingSystemContext();
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
        }
    }
}
