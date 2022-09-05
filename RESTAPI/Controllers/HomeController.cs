using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuaranteedRate;

namespace RESTAPI.Controllers
{
    public class HomeController : Controller
    {
        List<Person> people = new List<Person>();
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeController/Details/5
        [HttpGet]
        public ActionResult Records(string sort)
        {
            switch(sort)
            {
                case "color":
                    people.ForEach(p => p.SetSortOption(PeopleSortOption.ColorThenLastName));
                    people.Sort();
                    return View();
                case "birthdate":
                    people.ForEach(p => p.SetSortOption(PeopleSortOption.BirthDate));
                    people.Sort();
                    return View();
                case "name":
                    people.ForEach(p => p.SetSortOption(PeopleSortOption.LastNameDesc));
                    people.Sort();
                    return View();
                default:
                    //How to produce an error?
                    return View();
            }
        }

        // POST: HomeController/Create
        [HttpPost]
        [ActionName("Records")]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecord(string line)
        {
            Functionality.AddLineToPeople(line, people);
            return View();
        }
    }
}
