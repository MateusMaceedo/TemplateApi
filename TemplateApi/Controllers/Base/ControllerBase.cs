using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApi.Controllers.Base
{
    public class ControllerBase_ : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
