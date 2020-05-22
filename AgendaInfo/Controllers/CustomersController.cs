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
    public class CustomersController : Controller
    {
        private readonly IUserDAL userDAL;
        private readonly BDDContext _context;
        
        public CustomersController(IUserDAL _userDAL, BDDContext context)
        {
            userDAL = _userDAL;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
            {
                // 1. Création de l'utilisateur temporaire
                User tmpUser = new User(HttpContext.Session.GetString("userEmail"));
                tmpUser.LoadUserByEmail(userDAL);
                
                // 2. Si c'est l'admin on le renvoit vers la liste des utilisateurs
                if (IsAdmin(tmpUser.Email)) return View("ListCustomers", await _context.User.ToListAsync() as IEnumerable<User>);

                // 3. Viewbag pour afficher son prénom sur l'index du client
                else
                {
                    ViewBag.FirstName = tmpUser.FirstName;
                    return View(); 
                }
            }
            return RedirectToAction("Index", "Home");            
        }

        public IActionResult Details(int? id)
        {
            // 1. Si l'ID fourni est null on retourne non trouvé (404)
            if (id == null) return NotFound();

            // 2. Sinon on charge le client depuis la bdd
            Customer customer;
            customer = (Customer)userDAL.Get((int)id);

            //3. Si le client (id) n'existe pas dans la bdd on retourne non trouvé (404)
            if (customer == null) return NotFound();
            return View(customer);
        }

        public IActionResult Create()
        {
            // Si l'utilisateur est déjà connecté que ce n'est pas un admin on lui interdit la création de compte
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail"))) 
                if(!IsAdmin(HttpContext.Session.GetString("userEmail"))) 
                    return RedirectToAction("Index", "Home");
            return View();
        }                        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,Name,FirstName,Birthday,Address,PhoneNumber,Email,Password, ConfirmPassword")] Customer customer)
        {
            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                if (!customer.Exist(userDAL))
                { // Si il n'a pas trouvé l'email dans la BDD, on en crée un
                    customer.Register(userDAL);   // On enregistre le client
                    if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) HttpContext.Session.SetString("userEmail", customer.Email);  // On ajoute l'email  de l'utilisateur dans la session
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Message = "Ce pseudo existe déjà dans notre base de donnée !";
                    return View(customer);
                }             
            }
            return View(customer);
        }

        public IActionResult Edit(int? id)
        {
            // 1. Si l'ID fourni est null on retourne non trouvé (404)
            if (id == null) return NotFound();

            // 2. Sinon on charge le client depuis la bdd
            Customer customer;
            customer = (Customer)userDAL.Get((int)id);

            //3. Si le client (id) n'existe pas dans la bdd on retourne non trouvé (404)
            if (customer == null) return NotFound();
            //4. On check si l'utilisateur est admin pour ne pas lui proposer d'editer le mdp
            ViewBag.IsAdmin = false;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                if (IsAdmin(HttpContext.Session.GetString("userEmail")))
                    ViewBag.IsAdmin = true;
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,Name,FirstName,Birthday,Address,PhoneNumber,Email,Password")] Customer customer)
        {
            if (id != customer.ID) return NotFound();
            if (ModelState.IsValid)  {

                try { customer.Update(userDAL); }
                catch (DbUpdateConcurrencyException)
                {
                    if (customer.Exist(userDAL)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
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
