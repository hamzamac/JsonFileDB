using JsonFileDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestJsonFileDB.Models;

namespace TestJsonFileDB.DB
{
    public class AppDBContext : DBContext
    {
        public AppDBContext() : base(@".\Data\database.json")
        {
            Persons = new Dataset<Person>(_database);
        }

        public Dataset<Person> Persons { get; set; }
    }
}
