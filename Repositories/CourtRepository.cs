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
        //private readonly CourtDAO courtDAO;

        //public CourtRepository(CourtDAO courtDAO)
        //{
        //    this.courtDAO = courtDAO;
        //}

        public void AddCourt(Court court)
        {
            CourtDAO.AddCourt(court);
        }

        public void DeleteCourt(int id)
        {
            CourtDAO.DeleteCourt(id);
        }

        public List<Court> GetAllCourts()
        {
            return CourtDAO.GetAllCourts();
        }

        public Court GetCourtById(int id)
        {
            return CourtDAO.GetCourtById(id);
        }

        public void UpdateCourt(Court court)
        {
            CourtDAO.UpdateCourt(court);
        }
    }
}
