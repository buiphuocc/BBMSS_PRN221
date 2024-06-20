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
        private readonly BadmintonBookingSystemContext _context;

        public ServiceDAO(BadmintonBookingSystemContext context)
        {
            _context = context;
        }

        public List<Service> GetAllServices()
        {
            return _context.Services.ToList();
        }

        public Service GetServiceById(int id)
        {
            return _context.Services.Find(id);
        }

        public void AddService(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public void UpdateService(Service service)
        {
            _context.Entry(service).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteService(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
        }
    }
}
