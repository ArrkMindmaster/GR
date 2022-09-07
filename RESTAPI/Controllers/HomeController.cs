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
    public class HomeController : Controller, IPeopleAPI
    {
        readonly IAPIData people;
        public HomeController(IAPIData dataSource)
        {
            people = dataSource;
        }
        public HomeController()
        {
            people = new RealData();    //Can't figure out how to make this a parameter of the Program.cs, so making it default
        }

        [HttpGet("{sort}")]
        public IActionResult Get(string sortOption)
        {
            if(sortOption.Length<1)
            {
                return BadRequest("No sort specified.");
            }
            PeopleSortOption sort;
            switch (sortOption)
            {
                case "color":
                    sort = PeopleSortOption.ColorThenLastName;
                    break;
                case "birthdate":
                    sort = PeopleSortOption.BirthDate;
                    break;
                case "name":
                    sort = PeopleSortOption.LastNameDesc;
                    break;
                default:
                    //How to produce an error?
                    return BadRequest($"Invalid sort selected ({sortOption})");
            }
            string s=JsonConvert.SerializeObject(people.SortedList(sort));
            return Ok(s);
        }

        // POST: HomeController/Create
        [HttpPost]
        public IActionResult Post([FromBody] string line)
        {
            int startCount = people.Count;
            people.AddDataLine(line);
            if (startCount == people.Count)
            {
                return BadRequest($"{line} is invalid, and not added.");
            }
            else
            {
                return Ok($"{line} added!");
            }
        }
    }
}
