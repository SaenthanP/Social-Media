using System;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using PersonalNetworkService.Models;
using PersonalNetworkService.Services;
using System.Threading.Tasks;

public class UserNetworkService : IUserNetworkService
{
    private readonly IConfiguration _configuration;
    private readonly IDriver _driver;

    public UserNetworkService(IConfiguration configuration)
    {   
        _configuration=configuration;
        _driver=GraphDatabase.Driver(_configuration.GetSection("Neo4jHost").Value, AuthTokens.Basic(_configuration.GetSection("Neo4jUsername").Value, _configuration.GetSection("Neo4jPassword").Value));
    }
    public void AddUserToNetwork(MessageUserModel messageUserModel)
    {
             string message="Test";
             var session=_driver.Session();
             var addUser=session.WriteTransaction(tx=>{
                    var result = tx.Run("CREATE (a:Greeting) " +
                                    "SET a.message = $message " +
                                    "RETURN a.message + ', from node ' + id(a)",
                    new {message});
             });
                
            Console.WriteLine(addUser);
        throw new System.NotImplementedException();
    }
}
