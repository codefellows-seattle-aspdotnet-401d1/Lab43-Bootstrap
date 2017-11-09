using Lab28Tom.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab28Tom.Controllers
{
    public class HomeController : Controller
    {
        //private field, used for Dependendcy Injection
        private readonly Lab28TomContext _context;

        //Constructor for Home Controller - a DBContext is required
        public HomeController(Lab28TomContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //Getting content from all IDs in LFG table
            var result = _context.LFG.Where(c => c.ID != 0);


            return View(result.ToList());
        }
    }
}
