using System;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using PersonalNetworkService.Models;
using PersonalNetworkService.Services;
using System.Threading.Tasks;
using Neo4jClient;

public class UserNetworkService : IUserNetworkService
{
    private readonly IConfiguration _configuration;
    private readonly IDriver _driver;

    public UserNetworkService(IConfiguration configuration,IDriver driver)
    {   
        _configuration=configuration;
        _driver=driver;
    }
    public async Task AddUserToNetwork(MessageUserModel messageUserModel)
    {
        Console.WriteLine("reach");
        var session=_driver.AsyncSession();
        var query="Create(u:User {Id: '"+messageUserModel.Id+"', Username: '"+messageUserModel.Username+"', Email: '"+messageUserModel.Email+"'})";

        try{
            IResultCursor cursor = await session.RunAsync(query);
            await cursor.ConsumeAsync();
        }

        finally
        {
            await session.CloseAsync();
        }

        await _driver.CloseAsync();

    }
}
