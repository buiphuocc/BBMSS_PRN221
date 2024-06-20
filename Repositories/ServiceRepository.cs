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
    public class ServiceRepository : IServiceRepository
    {
        private readonly ServiceDAO serviceDAO;

        public ServiceRepository(ServiceDAO serviceDAO)
        {
            this.serviceDAO = serviceDAO;
        }
        public void AddService(Service service)
        {
            serviceDAO.AddService(service);
        }

        public void DeleteService(int id)
        {
            serviceDAO.DeleteService(id);
        }

        public List<Service> GetAllServices()
        {
            return serviceDAO.GetAllServices();
        }

        public Service GetServiceById(int id)
        {
            return (serviceDAO.GetServiceById(id));
        }

        public void UpdateService(Service service)
        {
            serviceDAO.UpdateService(service);
        }
    }
}
