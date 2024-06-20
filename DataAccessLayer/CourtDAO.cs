using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CourtDAO
    {
        private readonly BadmintonBookingSystemContext _context;

        public CourtDAO(BadmintonBookingSystemContext context)
        {
            _context = context;
        }

        public List<Court> GetAllCourts()
        {
            return _context.Courts.ToList();
        }

        public Court GetCourtById(int id)
        {
            return _context.Courts.Find(id);
        }

        public void AddCourt(Court court)
        {
            _context.Courts.Add(court);
            _context.SaveChanges();
        }

        public void UpdateCourt(Court court)
        {
            _context.Entry(court).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteCourt(int id)
        {
            var court = _context.Courts.Find(id);
            if (court != null)
            {
                _context.Courts.Remove(court);
                _context.SaveChanges();
            }
        }
    }
}
