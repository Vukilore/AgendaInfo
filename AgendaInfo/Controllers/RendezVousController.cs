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
    public class RendezVousController : Controller
    {
        private readonly BDDContext _context;
        private readonly IRendezVousDAL rdvDAL;
        private readonly IUserDAL userDAL;
        private readonly IServicesDAL serviceDAL;
        
        public RendezVousController(BDDContext context, IRendezVousDAL _rdvDAL, IUserDAL _userDAL, IServicesDAL _servicesDAL)
        {
            _context = context;
            rdvDAL = _rdvDAL;
            userDAL = _userDAL;
            serviceDAL = _servicesDAL;
        }

        // GET: RendezVous
        public IActionResult Index()
        {
            return View("../Agenda/Index");
        }

        // GET: RendezVous/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendezVous = await _context.RendezVous
                .FirstOrDefaultAsync(m => m.ID == id);
            if (rendezVous == null)
            {
                return NotFound();
            }

            return View(rendezVous);
        }

        // GET: RendezVous/Create
        public IActionResult Create(DateTime time)
        {
            //1. On récupère l'utilisateur courant
            User currentUser = new User(HttpContext.Session.GetString("userEmail"));
            currentUser = currentUser.LoadUserByEmail(userDAL);

            //2. On Stock la date de rendez vous
            ViewBag.ReservedDate = time;

            //2. On récupère les utilisateurs
            List<User> listUser = new List<User>();
            listUser = userDAL.GetAll();

            //3. On récupère les services
            foreach (User user in listUser)
                if (user is Admin) // Si on a trouvé l'admin dans la liste des utilisateurs todo : get directe l'admin
                {
                    Admin tmpUser = (Admin)user;
                    ViewBag.ListServices = tmpUser.ListServices;
                }

            //3. Si il est admin on stock la liste
            if (currentUser is Admin)
            {
                // 3.1 On enlève l'admin de la liste (forcément currentUser dans ce cas)
                listUser.Remove(currentUser);
                // 3.2 On stock la liste des utilisateurs dans un Viewbag sous forme de SelectList
                ViewBag.ListCustomers = listUser;
                // 3.3 On déclarera qu'il s'agit d'une vue pour l'admin (todo vue partiel)
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
                ViewBag.Customer = (Customer)currentUser;
            }  
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,Comment,BeginDate, Customer, Service")] RendezVous rendezVous)
        {
            if (ModelState.IsValid)
            {
                // HACK: Vu que le binding ne fonctionne pas on utilise un petit hack pour contourner ceci:
                // On utilise la méthode Request.Form pour rechercher la value Customer du formulaire
                // Au lieu d'obtenir l'objet en lui même, on va chercher son ToString
                // On va couper le string pour ne garder que l'email de l'utilisateur (qui se situe après le mark  '|' )
                // Et charger dans la bdd le client correspondant à l'email
                // TODO: Fixer le bind pour être plus optimisé
                User tmpCustomer = new User(Request.Form["Customer"].ToString().Remove(0, Request.Form["Customer"].ToString().IndexOf("|") + 2));
                rendezVous.Customer = (Customer)tmpCustomer.LoadUserByEmail(userDAL);

                // HACK: on utilise le même hack pour les services :
                Service tmpService = new Service(Convert.ToInt32(Request.Form["Service"].ToString().Remove(0, Request.Form["Service"].ToString().IndexOf("|") + 2)));
                rendezVous.Service = tmpService.LoadServiceByID(serviceDAL);

                // On sauvegarde le rendez-vous
                Agenda.Models.POCO.Agenda.GetInstance().AddRendezVous(rendezVous, rdvDAL);
                ViewBag.rendezvous = rendezVous;
                return View("Succeed");
            }
            return View(rendezVous);
        }

        // GET: RendezVous/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendezVous = await _context.RendezVous.FindAsync(id);
            if (rendezVous == null)
            {
                return NotFound();
            }
            return View(rendezVous);
        }

        // POST: RendezVous/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Comment,BeginDate")] RendezVous rendezVous)
        {
            if (id != rendezVous.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rendezVous);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RendezVousExists(rendezVous.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rendezVous);
        }

        // GET: RendezVous/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendezVous = await _context.RendezVous
                .FirstOrDefaultAsync(m => m.ID == id);
            if (rendezVous == null)
            {
                return NotFound();
            }

            return View(rendezVous);
        }

        // POST: RendezVous/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rendezVous = await _context.RendezVous.FindAsync(id);
            _context.RendezVous.Remove(rendezVous);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RendezVousExists(int id)
        {
            return _context.RendezVous.Any(e => e.ID == id);
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
