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
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Agenda");
        }

        public IActionResult Create(DateTime time)
        {
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) Redirect("../Customer/Index");
            ViewBag.ReservedDate = time;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]                                                    
        public IActionResult Create([Bind("ID,StartDate, Reason")] DayOff dayOff)
        {
            if (ModelState.IsValid)
            {
                if (dayOff.Reason == null) dayOff.Reason = "Aucune";
                Agenda.Models.POCO.Agenda.GetInstance().AddDayOff(dayOff, dayOffDAL);
                return RedirectToAction("Index","Agenda");        
            }
            return View(dayOff);
        }


        public IActionResult Delete(int? id)
        {
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) Redirect("../Customer/Index");
            if (id == null)
            {
                return NotFound();
            }
            Agenda.Models.POCO.Agenda.GetInstance().Update(dayOffDAL);
            DayOff tmpDayOff = new DayOff();
            tmpDayOff= Agenda.Models.POCO.Agenda.GetInstance().ListDaysOff.Find(d => d.ID == id);
            if (tmpDayOff == null)
            {
                return NotFound();
            }

            return View(tmpDayOff);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) Redirect("../Customer/Index");
            Agenda.Models.POCO.Agenda.GetInstance().Update(dayOffDAL);
            DayOff tmpDayOff = new DayOff();
            tmpDayOff = Agenda.Models.POCO.Agenda.GetInstance().ListDaysOff.Find(d => d.ID == id);
            Agenda.Models.POCO.Agenda.GetInstance().DeleteDayOff(tmpDayOff, dayOffDAL);
            return RedirectToAction("Index", "Agenda");
        }

        /*=========================================
        * IsAdmin: Retourne true si l'email fourni est celui de l'admin
        *=========================================*/
        [NonAction]
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
