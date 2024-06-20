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
    public class CourtRepository : ICourtRepository
    {
        public bool CreateCourt(Court court)
            => CourtDAO.CreateCourt(court);

        public Court? GetCourtById(int id)
            => CourtDAO.GetCourtById(id);

        public List<Court> GetCourts()
            => CourtDAO.GetCourts();

        public bool UpdateCourt(Court court)
            => CourtDAO.UpdateCourt(court);
    }
}
