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
        //private readonly BadmintonBookingSystemContext _context;

        //public CourtDAO(BadmintonBookingSystemContext context)
        //{
        //    _context = context;
        //}

        public static List<Court> GetAllCourts()
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Courts.ToList();
        }

        public static Court GetCourtById(int id)
        {
            using var _context = new BadmintonBookingSystemContext();
            return _context.Courts.Find(id);
        }

        public static void AddCourt(Court court)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Courts.Add(court);
            _context.SaveChanges();
        }

        public static void UpdateCourt(Court court)
        {
            using var _context = new BadmintonBookingSystemContext();
            _context.Entry(court).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public static void DeleteCourt(int id)
        {
            using var _context = new BadmintonBookingSystemContext();
            var court = _context.Courts.Find(id);
            if (court != null)
            {
                _context.Courts.Remove(court);
                _context.SaveChanges();
            }
        }
    }
}
