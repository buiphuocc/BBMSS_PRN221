using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICourtService
    {
        List<Court> GetAllCourts();

        Court GetCourtById(int id);

        void AddCourt(Court court);

        void UpdateCourt(Court court);

        void DeleteCourt(int id);
    }
}
