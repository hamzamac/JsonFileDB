using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonFileDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestJsonFileDB.DB;
using TestJsonFileDB.Models;

namespace TestJsonFileDB.Controllers
{
    [Produces("application/json")]
    [Route("api/Person")]
    public class PersonController : Controller
    {
        private AppDBContext _db;

        public PersonController(IDBContext db) => _db = (AppDBContext)db;

        // GET: api/Person
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return _db.Persons.GetAll();
        }

        // GET: api/Person/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Person> Get(int id)
        {
            return await _db.Persons.FindAsync(id);
        }
        
        // POST: api/Person
        [HttpPost]
        public IActionResult Post([FromBody]Person person)
        {
            if (person == null)
            {
                BadRequest();
            }
            _db.Persons.Add(person);
            _db.SaveChanges();
            return new NoContentResult();
        }
        
        // PUT: api/Person/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Person newPerson)
        {
           if (newPerson == null || newPerson.Id != id)
            {
                BadRequest();
            }
            var oldPerson = _db.Persons.Find(id);
            if (oldPerson == null)
            {
                NotFound();
            }
            _db.Persons.Update(newPerson);
            _db.SaveChanges();

            return new NoContentResult();
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var person = _db.Persons.Find(id);
            if (person == null)
            {
                BadRequest();
            }
            _db.Persons.Remove(id);
            _db.SaveChanges();
            return new NoContentResult();
        }
    }
}
