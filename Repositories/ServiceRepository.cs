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
        //private readonly ServiceDAO serviceDAO;

        //public ServiceRepository(ServiceDAO serviceDAO)
        //{
        //    this.serviceDAO = serviceDAO;
        //}
        public void AddService(Service service)
            => ServiceDAO.AddService(service);

        public void DeleteService(int id)
            => ServiceDAO.DeleteService(id);

        public List<Service> GetAllServices()
            =>ServiceDAO.GetAllServices();

        public Service GetServiceById(int id)
            =>ServiceDAO.GetServiceById(id);

        public void UpdateService(Service service)
            => ServiceDAO.UpdateService(service);
    }
}
