using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agenda.Models.POCO;
using AgendaInfo.DATA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgendaInfo.Controllers
{
    public class AgendaController : Controller
    {
        private readonly IRendezVousDAL rdvDAL;
        public AgendaController(IRendezVousDAL _rdvDAL)
        {
            rdvDAL = _rdvDAL;
        }
        public ActionResult Index(int Week = 0)
        {
            // 1. On défini la semaine à gérer depuis 'aujourd'hui + les semaines à regarder' 
            DateTime WeekToShow = DateTime.Now.AddDays(7 * Week);

            //2. On trouve le lundi de la semaine à gérer
            DateTime MondayOfWeek = WeekToShow.AddDays(-(int)WeekToShow.DayOfWeek + (int)DayOfWeek.Monday);
            ViewBag.MondayOfWeek = MondayOfWeek;

            //3. On crée une liste des RDV de la semaine à gérer
            List<RendezVous> RdvThisWeek = new List<RendezVous>();

            //4. On récupère tous les rdv
            Agenda.Models.POCO.Agenda.GetInstance().UpdateRDV(rdvDAL);
            List<RendezVous> ListRendezVous = Agenda.Models.POCO.Agenda.GetInstance().ListRendezVous;

            //5. Pour chaque rdv dans la liste de l'agenda
            foreach (RendezVous rdv in ListRendezVous)
                //5.1. Si le rendez-vous se situe dans la semaine on l'ajoute à la liste
                if (rdv.BeginDate.Day >= MondayOfWeek.Day && rdv.BeginDate <= (MondayOfWeek.AddDays(7)))
                    RdvThisWeek.Add(rdv);
            ViewBag.CurrentWeek = Week;
            return View(RdvThisWeek);
        }

        // GET: Agenda/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Agenda/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agenda/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Agenda/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Agenda/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Agenda/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Agenda/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}