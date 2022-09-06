using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuaranteedRate;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net;

namespace RESTAPI.Controllers
{
    [Route("api/people"), ApiController]
    public class HomeController : Controller
    {
        static List<Person> people = new List<Person>();

        [HttpGet("{sort}")]
        public IActionResult Get(string sort)
        {
            string peopleMessage = $"{people.Count} people stored.\n";
            peopleMessage = "";
            switch (sort)
            {
                case "color":
                    people.ForEach(p => p.SetSortOption(PeopleSortOption.ColorThenLastName));
                    people.Sort();
                    break;
                case "birthdate":
                    people.ForEach(p => p.SetSortOption(PeopleSortOption.BirthDate));
                    people.Sort();
                    break;
                case "name":
                    people.ForEach(p => p.SetSortOption(PeopleSortOption.LastNameDesc));
                    people.Sort();
                    break;
                default:
                    //How to produce an error?
                    return StatusCode(500, $"Invalid sort selected ({sort})");
            }
            return StatusCode(200, peopleMessage + JsonConvert.SerializeObject(people));
        }

        // POST: HomeController/Create
        [HttpPost]
        public IActionResult Post([FromBody] string line)
        {
            Functionality.AddLineToPeople(line, people);
            return StatusCode(200, $"{line} added!");
        }
    }
}
