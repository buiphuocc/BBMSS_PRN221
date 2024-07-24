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
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }
        public void AddService(Service service)
        {
            serviceRepository.AddService(service);
        }

        public void DeleteService(int id)
        {
            serviceRepository.DeleteService(id);
        }

        public List<Service> GetAllActiveServices()
        {
            return serviceRepository.GetAllActiveServices();
        }

        public List<Service> GetAllServices()
        {
            return serviceRepository.GetAllServices();
        }

        public Service GetServiceById(int id)
        {
            return serviceRepository.GetServiceById(id);
        }

        public void UpdateService(Service service)
        {
            serviceRepository.UpdateService(service);
        }
    }
}
