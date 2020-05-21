using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agenda.Models.POCO;
using AgendaInfo.DATA;
using SQLitePCL;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;

namespace AgendaInfo.Controllers
{
    public class ServicesController : Controller
    {
        private readonly BDDContext _context;
        private readonly IUserDAL userDAL;
        private readonly IServicesDAL serviceDAL;

        public ServicesController(BDDContext context, IUserDAL _userDAL, IServicesDAL _serviceDAL)
        {
            _context = context;
            userDAL = _userDAL;
            serviceDAL = _serviceDAL;
        }

        // GET: Services
        public IActionResult Index()
        {
            // 1. Chargement de l'administrateur
            Admin tmpadmin = new Admin();
            tmpadmin = tmpadmin.GetAdmin(userDAL);

            // 2. Vérifier si l'utilisateur courant est administrateur
            if (IsAdmin(HttpContext.Session.GetString("userEmail")))
                ViewBag.IsAdmin = true;

            // 3. Retourne la vue des services
            return View(tmpadmin.ListServices);
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Service
                .FirstOrDefaultAsync(m => m.ID == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) Redirect("../Customer/Index");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,Name,Price,Duration")] Service service)
        {
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) Redirect("../Customer/Index");
            if (ModelState.IsValid)
            {
                Admin tmpadmin = new Admin();
                tmpadmin = tmpadmin.GetAdmin(userDAL);
                tmpadmin.AddService(service, userDAL);
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public IActionResult Delete(int? id)
        {
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) RedirectToAction(nameof(Index));
            if (id == null)
            {
                return NotFound();
            }
            Admin tmpadmin = new Admin();
            tmpadmin = tmpadmin.GetAdmin(userDAL);
            Service tmpservice = new Service();
            tmpservice = tmpadmin.ListServices.Find(s => s.ID == id);
            if (tmpservice == null)
                return NotFound();
            return View(tmpservice);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) RedirectToAction(nameof(Index));
            Admin tmpadmin = new Admin();
            tmpadmin = tmpadmin.GetAdmin(userDAL);
            Service tmpservice = new Service();
            tmpservice = tmpadmin.ListServices.Find(s => s.ID == id);
            tmpadmin.DeleteService(tmpservice, userDAL);
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.ID == id);
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
