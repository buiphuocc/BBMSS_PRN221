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
    public class CourtService : ICourtService
    {
        private readonly ICourtRepository courtRepository;

        public CourtService(ICourtRepository courtRepository)
        {
            this.courtRepository = courtRepository;
        }

        public void AddCourt(Court court)
        {
            courtRepository.AddCourt(court);
        }

        public void DeleteCourt(int id)
        {
            courtRepository.DeleteCourt(id);
        }

        public List<Court> GetAllActiveCourts()
        {
            return courtRepository.GetAllActiveCourts();
        }

        public List<Court> GetAllCourts()
        {
            return courtRepository.GetAllCourts();
        }

        public Court GetCourtById(int id)
        {
            return courtRepository.GetCourtById(id);
        }

        public void UpdateCourt(Court court)
        {
            courtRepository.UpdateCourt(court);
        }
    }
}
