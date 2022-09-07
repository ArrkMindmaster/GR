using GuaranteedRate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RESTAPI;
using RESTAPI.Controllers;
using System.Collections.Generic;
using System.Net;
using Xunit.Sdk;

namespace TestAPI
{
    [TestClass]
    public class APITests
    {
        [TestMethod]
        public void GetReturnsOK()
        {
            HomeController client = new HomeController(new RealData());  //For this purpose, it isn't REAL data, since it isn't actually in a database
            var result = client.Get("color");
            Assert.IsTrue((int)HttpStatusCode.OK == (result as OkObjectResult).StatusCode);
            result = client.Get("birthdate");
            Assert.IsTrue((int)HttpStatusCode.OK == (result as OkObjectResult).StatusCode);
            result = client.Get("name");
            Assert.IsTrue((int)HttpStatusCode.OK == (result as OkObjectResult).StatusCode);
        }
        [TestMethod]
        public void GetInvalidSortReturns400()
        {
            HomeController client = new HomeController(new RealData());
            var result = client.Get("");
            Assert.AreEqual(400, (result as BadRequestObjectResult).StatusCode);
            result = client.Get("1");
            Assert.AreEqual(400, (result as BadRequestObjectResult).StatusCode);
            result = client.Get("sort");
            Assert.AreEqual(400, (result as BadRequestObjectResult).StatusCode);
            result = client.Get("birth date");
            Assert.AreEqual(400, (result as BadRequestObjectResult).StatusCode);
            result = client.Get("nameasc");
            Assert.AreEqual(400, (result as BadRequestObjectResult).StatusCode);
        }
        [TestMethod]
        public void PostAddsLinesWithoutAddingLinesWithBadData()
        {
            IAPIData data = new RealData();
            HomeController client = new HomeController(data);
            var result = client.Post("Person|First|fp@gr.com|Red|1/1/2000");
            Assert.AreEqual(1, data.Count);
            Assert.IsTrue((int)HttpStatusCode.OK == (result as OkObjectResult).StatusCode);
            Assert.IsTrue((result as OkObjectResult).Value.ToString().Contains("added!"));
            result = client.Post("Person,Second,sp@gr.com,White,1/2/2000");
            Assert.AreEqual(2, data.Count);
            Assert.IsTrue((int)HttpStatusCode.OK == (result as OkObjectResult).StatusCode);
            Assert.IsTrue((result as OkObjectResult).Value.ToString().Contains("added!"));
            result = client.Post("Invalid Client Birth Date 2/71/2000");
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(400, (result as BadRequestObjectResult).StatusCode);
            Assert.IsTrue((result as BadRequestObjectResult).Value.ToString().Contains("invalid"));
            result = client.Post("Another Person ap@gr.com Red 1/3/2000");
            Assert.AreEqual(3, data.Count);
            Assert.IsTrue((int)HttpStatusCode.OK == (result as OkObjectResult).StatusCode);
            Assert.IsTrue((result as OkObjectResult).Value.ToString().Contains("added!"));
            result = client.Post("Invalid due to far too many spaces in the line, or too few commas, || too few pipes.");
            Assert.AreEqual(3, data.Count);
            Assert.AreEqual(400, (result as BadRequestObjectResult).StatusCode);
            Assert.IsTrue((result as BadRequestObjectResult).Value.ToString().Contains("invalid"));
        }
        [TestMethod]
        public void GetReturnsCorrectSorts()
        {
            IAPIData data = new RealData();
            HomeController client = new HomeController(data);
            client.Post("Person|First|fp@gr.com|Red|1/1/2000");
            client.Post("Person,Second,sp@gr.com,White,1/2/2000");
            client.Post("Another Person ap@gr.com Red 1/3/2000");
            client.Post("Girl,That,tg@gr.com,Blue,1/4/1999");
            client.Post("Guy,Another,ag@gr.com,White,1/5/1999");

            var result = client.Get("color");
            List<Person> people = JsonConvert.DeserializeObject<List<Person>>((result as OkObjectResult).Value.ToString());

            Assert.AreEqual(5, people.Count);
            Assert.AreEqual("tg@gr.com", people[0].Email);
            Assert.AreEqual("ap@gr.com", people[1].Email);
            Assert.AreEqual("fp@gr.com", people[2].Email);
            Assert.AreEqual("ag@gr.com", people[3].Email);
            Assert.AreEqual("sp@gr.com", people[4].Email);

            result = client.Get("birthdate");
            people = JsonConvert.DeserializeObject<List<Person>>((result as OkObjectResult).Value.ToString());

            Assert.AreEqual(5, people.Count);
            Assert.AreEqual("tg@gr.com", people[0].Email);
            Assert.AreEqual("ag@gr.com", people[1].Email);
            Assert.AreEqual("fp@gr.com", people[2].Email);
            Assert.AreEqual("sp@gr.com", people[3].Email);
            Assert.AreEqual("ap@gr.com", people[4].Email);

            result = client.Get("name");
            people = JsonConvert.DeserializeObject<List<Person>>((result as OkObjectResult).Value.ToString());

            Assert.AreEqual(5, people.Count);
            Assert.AreEqual("fp@gr.com", people[0].Email);
            Assert.AreEqual("sp@gr.com", people[1].Email);
            Assert.AreEqual("ag@gr.com", people[2].Email);
            Assert.AreEqual("tg@gr.com", people[3].Email);
            Assert.AreEqual("ap@gr.com", people[4].Email);
        }
    }
}
