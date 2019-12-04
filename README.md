# JsonFileDataBase
__Version 1.2.0__  
Dovnload [NuGet](https://www.nuget.org/packages/JsonFileDataBase/)

> `PM> Install-Package JsonFileDataBase -Version 1.0.3 `  

See [Sample Code](https://github.com/hamzamac/Asolvi.People/tree/WithoutDBLibrary)

The following is a demo fo use of JsonFileDataBase in a C# web project
## Create the data model
Your model classes should inherit from `ITable` and thus must contain the `Id` propoerty.
```
    public class Person : ITable
    {
        public int Id { get; set; }
        //add other propoerties
    }

    public class Location :ITable
    {
        public int Id { get; set; }
        //add other propoerties
    }
```

## Create the Database Context
The main class that coordinates `JsonFileDataBase` functionality for a given data model is the database context class. You create this class by deriving from the `JsonFileDB.DbContext` class. In your code you specify which entities are included in the data model.  

The DBContect constructor requires a `string` of the path to the JSON file to be uses as database.  

```
    public AppDBContext() 
    :base("jsonFilePath") //can be provided as string literal
    {

    }
```
>OR alternatively
The path can be set in the app setting file and injected from configuration service using dependancy injection
```
    public AppDBContext(IConfiguration configuration) 
    :base(configuration.GetConnectionString("JSONFilePath")) 
    {

    }
```
Add the Models inthe Database context: regester the classes as tables
```
    public class AppDBContext : DBContext
    { 
        public AppDBContext() 
        :base("jsonFilePath") //can be provided as string literal
        {
        }
        //declare all table intstances
        public Dataset<Person> Persons { get; set; }
        public Dataset<Location> Locations { get; set; }
    }
```
This code creates a DbSet property for each entity set. In `JsonFileDataBase` terminology, an entity set typically corresponds to a database table, and an entity corresponds to a row in the table.

Finaly,initialize the table instances in the database; this generates the tables only once and if the do notexist.
```
    public AppDBContext() 
    :base("") 
    {
        Persons = new Dataset<Person>(_database);
        Locations = new Dataset<Location>(_database);
    }
```
The final code should look similar to this:
```
    public class AppDBContext : DBContext
    { 
        public AppDBContext() 
        :base("jsonFilePath") //can be provided as string literal
        {
            Persons = new Dataset<Person>(_database);
            Locations = new Dataset<Location>(_database);
        }
        //declare all table intstances
        public Dataset<Person> Persons { get; set; }
        public Dataset<Location> Locations { get; set; }
    }
```
When the database is created, `JsonFileDataBase` creates tables that have names the same as the Model class names all in lowercase.

## Register the context with dependency injection
ASP.NET Core implements dependency injection by default. Services (such as the `JsonFileDataBase` database context) are registered with dependency injection during application startup. Components that require these services (such as MVC controllers) are provided these services via constructor parameters. 

To register `AppDBContext` as a service, open `Startup.cs`, and add the `services.AddSingleton<IDBContext, AppDBContext>();` line to the ConfigureServices method.

```
    public void ConfigureServices(IServiceCollection services
    {       
        services.AddSingleton<IDBContext, AppDBContext>();
        services.AddMvc();
    }
```

## Register the DBContext in the controller class

```
    public class PersonController : Controller
    {
        private AppDBContext _db;

        public PersonController(IDBContext db)
        {
            _db = (AppDBContext) db;
        }
    }
```

From here the Database CRUD operation can be achieved 

>Example:
```
    _db.Persons.Add(person); //add person to database
    _db.Persons.GetAll();    //get all entities from the person table
    _db.Persons.Find(id);   //find a person with a specific id from the person table
    _db.Persons.Remove(id);  //remos the person with a specified id from the person table
    _db.Persons.Update(person); //updates the person
    _db.SaveChanges();       //saves changes to database
```

See [Sample Code](https://github.com/hamzamac/Asolvi.People/tree/WithoutDBLibrary)
