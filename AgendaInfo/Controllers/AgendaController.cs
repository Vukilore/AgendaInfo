using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Agenda.Models.POCO;
using AgendaInfo.DATA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgendaInfo.Controllers
{
    public class AgendaController : Controller
    {
        private readonly IRendezVousDAL rdvDAL;
        private readonly IUserDAL userDAL;
        public AgendaController(IRendezVousDAL _rdvDAL, IUserDAL _userDAL)
        {
            rdvDAL = _rdvDAL;
            userDAL = _userDAL;
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
            //6. On défini la semaine courrante et le numéro de la semaine dans l'année
            ViewBag.CurrentWeek = Week;
            ViewBag.WeekNumber = GetWeekNumber(MondayOfWeek);
            //7. On défini si l'utilisateur est l'admin ou non
            ViewBag.IsAdmin = IsAdmin(HttpContext.Session.GetString("userEmail"));
            return View(RdvThisWeek);
        }





        [NonAction]
        private bool IsAdmin(string email)
        {
            // 1. Création de l'utilisateur temporaire
            User tmpUser = new User(email);
            // 2. Chargement de l'utilisateur grâce à son email
            tmpUser = tmpUser.LoadUserByEmail(userDAL);
            return tmpUser is Admin;
        }
        [NonAction]
        public static int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        
    }
}