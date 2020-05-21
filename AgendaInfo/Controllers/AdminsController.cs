using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agenda.Models.POCO;
using AgendaInfo.DATA;

namespace AgendaInfo.Controllers
{
    public class AdminsController : Controller
    {
        public AdminsController()
        {
           
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
