using JsonFileDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestJsonFileDB.Models
{
    public class Address : ITable
    {
        public object Id { get ; set ; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
