using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models;

namespace App.Controllers
{
    public class FileController : Controller
    {
        
        public IActionResult Add()
        {
            ViewData["Message"] = "Add doc.";

            return View();
        } 
        
        [Route("[action]/{id}")]
        public IActionResult Edit(string id)
        {
            ViewData["Message"] = $"Edit doc {id}.";

            return View();
        }
        
        public IActionResult History()
        {
            ViewData["Message"] = "All docs.";

            return View();
        }
        
        [Route("[action]/{id}")]
        public IActionResult Info(string id)
        {
            ViewData["Message"] = $"Doc {id}.";

            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}