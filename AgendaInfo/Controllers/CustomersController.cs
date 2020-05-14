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

        public CustomersController(IUserDAL _userDAL)
        {
            userDAL = _userDAL;
        }

        // GET: Customers
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
            {
                // 1. Création de l'utilisateur temporaire
                Customer tmpUser = new Customer(HttpContext.Session.GetString("userEmail"));
                tmpUser.LoadUserByEmail(userDAL);
                return View();
            }
            return Redirect("../Home/Index");
            
        }

        // GET: Customers/Details/5
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
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail"))) return Redirect("../Home/Index");/*RedirectToAction(nameof(Index));*/
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
                { // Si il n'a pas trouvé le pseudo dans la BDD, on en crée un
                    customer.Register(userDAL);   // On enregistre le client TODO: exception utilisateur déjà créé
                    HttpContext.Session.SetString("userEmail", customer.Email);  // On ajoute l'email  de l'utilisateur dans la session
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Message = "Ce pseudo existe déjà dans notre base de donnée !";
                    return View("Create");
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
        /*
        public async Task<IActionResult> Delete(int? id)
        {
            // TODO: if session user is admin
            if (id == null) return NotFound();

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.ID == id);

            if (customer == null) return NotFound();

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/
    }
}
