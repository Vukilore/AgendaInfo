using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agenda.Models.POCO;
using AgendaInfo.DATA;
using Microsoft.AspNetCore.Http;

namespace AgendaInfo.Controllers
{
    public class DayOffController : Controller
    {
        private readonly BDDContext _context;
        private readonly IUserDAL userDAL;
        private readonly IDayOffDAL dayOffDAL;
        public DayOffController(BDDContext context, IUserDAL _userDAL, IDayOffDAL _dayOffDAL)
        {
            _context = context;
            userDAL = _userDAL;
            dayOffDAL = _dayOffDAL;
        }
        // GET: DayOff/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayOff = await _context.DayOff
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dayOff == null)
            {
                return NotFound();
            }

            return View(dayOff);
        }

        // GET: DayOff/Create
        public IActionResult Create(DateTime time)
        {
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) Redirect("../Customer/Index");
            ViewBag.ReservedDate = time;
            return View();
        }

        // POST: DayOff/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,StartDate, Reason")] DayOff dayOff)
        {
            if (ModelState.IsValid)
            {
                Agenda.Models.POCO.Agenda.GetInstance().AddDayOff(dayOff, dayOffDAL);
                return RedirectToAction("Details",dayOff.ID);
            }
            return View(dayOff);
        }

        // GET: DayOff/Delete/5
        public IActionResult Delete(int? id)
        {
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) Redirect("../Customer/Index");
            if (id == null)
            {
                return NotFound();
            }
            Agenda.Models.POCO.Agenda.GetInstance().UpdateDayOff(dayOffDAL);
            DayOff tmpDayOff = new DayOff();
            tmpDayOff= Agenda.Models.POCO.Agenda.GetInstance().ListDaysOff.Find(d => d.ID == id);
            if (tmpDayOff == null)
            {
                return NotFound();
            }

            return View(tmpDayOff);
        }
        // POST: DayOff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) Redirect("../Customer/Index");
            Agenda.Models.POCO.Agenda.GetInstance().UpdateDayOff(dayOffDAL);
            DayOff tmpDayOff = new DayOff();
            tmpDayOff = Agenda.Models.POCO.Agenda.GetInstance().ListDaysOff.Find(d => d.ID == id);
            Agenda.Models.POCO.Agenda.GetInstance().DeleteDayOff(tmpDayOff, dayOffDAL);

            return RedirectToAction(nameof(Index));
        }

        private bool DayOffExists(int id)
        {
            return _context.DayOff.Any(e => e.ID == id);
        }
        private bool IsAdmin(string email)
        {
            // 1. Création de l'utilisateur temporaire
            User tmpUser = new User(email);
            // 2. Chargement de l'utilisateur grâce à son email
            tmpUser = tmpUser.LoadUserByEmail(userDAL);
            return tmpUser is Admin;
        }
    }
}
