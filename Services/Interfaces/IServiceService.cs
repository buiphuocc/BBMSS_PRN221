using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        List<Service> GetAllServices();
        Service GetServiceById(int id);
        void AddService(Service service);
        void UpdateService(Service service);
        void DeleteService(int id);
    }
}
