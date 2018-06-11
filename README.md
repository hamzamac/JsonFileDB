# JsonFileDB
A Class libbray for use of JSON file as an application database 

See [sample code](https://github.com/hamzamac/Asolvi.People/tree/WithoutDBLibrary) with Web API implementatin

### Create model classes that implements ITable interface
These are the classes that represent your database tables in the Json file

    public class Location :ITable
    {
        public long Id { get; set; }
    }

### Create Database context class
Register your file to the context of the database

 1. create a class that extends DBContext
 

        public class ApplicationDBContext : DBContext{}
  
 2. define a constructor that with a string argument for the filepath of the target json file

    public ApplicationDBContext(string jsonFilePath):base(jsonFilePath){}

 3. Declare the Model classes as fields in the DBontext class as Datasets: i.e register the classes as tables of the database
 

        public Dataset<Person> Persons { get; set; }
        public Dataset<Location> Locations { get; set; }
  

 4. Initialise the Datasest in the DBContect constructor like this

        public ApplicationDBContext(string jsonFilePath):base(jsonFilePath)
        {
            Persons = new Dataset<Person>(_database);
            Locations = new Dataset<Location>(_database);
        }

 ### Register Dependancy Injection
 in the ConfigureServices() in the startup.cs file add DBContext to services
 

    services.AddSingleton<IDBContext, ApplicationDbContext>();


