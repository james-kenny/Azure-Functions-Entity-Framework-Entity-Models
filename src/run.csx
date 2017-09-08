#r "System.Data"

using System.Net;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Linq;
using System.Data.Entity;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    dynamic body = await req.Content.ReadAsStringAsync();
    var e = JsonConvert.DeserializeObject<Person>(body as string);
    e.Created_DT = System.DateTime.Now;
    
    try
    {        
        using (PeopleContext context = new PeopleContext())
        {
            context.Persons.Add(e); 
            context.SaveChanges();            
        }
    }
    catch(System.Data.Entity.Infrastructure.DbUpdateException ex)
    {
        log.Info(string.Format("Failure with database update {0}.", ex.Message));        
    }  
    

    return req.CreateResponse(HttpStatusCode.OK, "Ok");
}

public class PeopleContext : DbContext
{
    public PeopleContext()
        : base("XXX")
    { }


    public DbSet<Person> Persons { get; set; }
}

public class Person{
    public int Id { get; set; }
    public string FirstName_VC {get;set;}
    public string LastName_VC {get;set;}
    public string Email_VC {get;set;}
    public DateTime Created_DT {get;set;}
}