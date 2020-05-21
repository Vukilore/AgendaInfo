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
        private readonly IUserDAL userDAL;
        private readonly IServicesDAL serviceDAL;

        public ServicesController(IUserDAL _userDAL, IServicesDAL _serviceDAL)
        {
            userDAL = _userDAL;
            serviceDAL = _serviceDAL;
        }

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
        public IActionResult Details(int? id)
        {
            // 1. Si l'ID fourni est null, on retourne NotFound
            if (id == null) return NotFound();

            //2. On charge l'admin pour avoir sa liste de service
            Admin tmpadmin = new Admin();
            tmpadmin = tmpadmin.GetAdmin(userDAL);

            //3. On vérifie si l'ID fourni existe bien dans la liste
            Service tmpservice = new Service();
            tmpservice = tmpadmin.ListServices.Find(s => s.ID == id);

            //4. Si l'ID n'existe pas on retourne NotFound
            if (tmpservice == null) return NotFound();

            // 5. Si c'est un admin, on le déclare à la vue
            if (IsAdmin(HttpContext.Session.GetString("userEmail"))) ViewBag.IsAdmin = true;

            return View(tmpservice);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            // 1. Si l'utilisateur courant est pas admin, on le redirige
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
            // 1. Si l'utilisateur courant est pas admin, on le redirige
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) Redirect("../Customer/Index");

            // 2. Si le model est valide
            if (ModelState.IsValid)
            {
                // 2.1. On charge l'admin pour avoir sa liste de service
                Admin tmpadmin = new Admin();
                tmpadmin = tmpadmin.GetAdmin(userDAL);
                // 2.2. On ajoute le service à sa liste
                tmpadmin.AddService(service, userDAL);
                // 2.3 On redirige
                return RedirectToAction(nameof(Index));
            }
            // 3. Modèle pas valide on le ré-affiche
            return View(service);
        }

        // GET: Services/Delete/5
        public IActionResult Delete(int? id)
        {
            // 1. Si l'utilisateur courant est pas admin, on le redirige
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) RedirectToAction(nameof(Index));

            // 2. Si l'ID fourni est null, on retourne NotFound
            if (id == null) return NotFound();

            //3. On charge l'admin pour avoir sa liste de service
            Admin tmpadmin = new Admin();
            tmpadmin = tmpadmin.GetAdmin(userDAL);

            //4. On vérifie si l'ID fourni existe bien dans la liste
            Service tmpservice = new Service();
            tmpservice = tmpadmin.ListServices.Find(s => s.ID == id);

            //5. Si l'ID n'existe pas on retourne NotFound
            if (tmpservice == null) return NotFound();
            
            //6. Sinon on affiche la vue Delete
            return View(tmpservice);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // 1. Si l'utilisateur courant est pas admin, on le redirige
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) RedirectToAction(nameof(Index));

            // 2. On charge l'admin pour avoir sa liste de service
            Admin tmpadmin = new Admin();
            tmpadmin = tmpadmin.GetAdmin(userDAL);

            // 3. On obtient l'objet 'service' dans la liste depuis son ID
            Service tmpservice = new Service();
            tmpservice = tmpadmin.ListServices.Find(s => s.ID == id);

            // 4. On supprime le service de la liste
            tmpadmin.DeleteService(tmpservice, userDAL);

            // 5. On redirige vers la liste des services
            return RedirectToAction(nameof(Index));
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
