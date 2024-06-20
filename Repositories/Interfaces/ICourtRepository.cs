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
        List<Court> GetCourts();

        Court? GetCourtById(int id);

        bool CreateCourt(Court court);

        bool UpdateCourt(Court court);
    }
}
