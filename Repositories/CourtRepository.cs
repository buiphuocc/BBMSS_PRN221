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
        private readonly CourtDAO courtDAO;

        public CourtRepository(CourtDAO courtDAO)
        {
            this.courtDAO = courtDAO;
        }

        public void AddCourt(Court court)
        {
            courtDAO.AddCourt(court);
        }

        public void DeleteCourt(int id)
        {
            courtDAO.DeleteCourt(id);
        }

        public List<Court> GetAllCourts()
        {
            return courtDAO.GetAllCourts();
        }

        public Court GetCourtById(int id)
        {
            return courtDAO.GetCourtById(id);
        }

        public void UpdateCourt(Court court)
        {
            courtDAO.UpdateCourt(court);
        }
    }
}
