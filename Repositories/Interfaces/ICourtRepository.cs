using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICourtRepository
    {
        List<Court> GetAllCourts();

        Court GetCourtById(int id);

        void AddCourt(Court court);

        void UpdateCourt(Court court);

        void DeleteCourt(int id);
    }
}
