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
        try{
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

        // await _driver.CloseAsync();
        }catch(Exception ex){
            Console.WriteLine(ex);
        }

    }

    public async Task FollowUser(string userToFollowId,string userId)
    {
        var session=_driver.AsyncSession();
        var query="MATCH (u1:User),(u2:User) WHERE u1.Id='"+userId+"' AND u2.Id='"+userToFollowId+"' MERGE (u1)-[:`follows`]->(u2)";

        try{
            IResultCursor cursor = await session.RunAsync(query);
            await cursor.ConsumeAsync();
        }

        finally
        {
            await session.CloseAsync();
        }

    }

    public async Task<bool> IsFollowingUser(string userToCheck, string userId)
    {
        var session=_driver.AsyncSession();
        var query="MATCH (u1:User),(u2:User) WHERE u1.Id='"+userId+"' AND u2.Id='"+userToCheck+"' AND (u1)-[:`follows`]->(u2) RETURN u2";

      
        try{
            IResultCursor cursor = await session.RunAsync(query);
            await cursor.FetchAsync();
            var result=cursor.Current;
            if(result!=null){
                return true;
            }
            
        } finally{
            await session.CloseAsync();
        }

        return false;
    }

    public async Task UnfollowUser(string userToFollowId, string userId)
    {
        var session=_driver.AsyncSession();
        var query="MATCH (u1:User),(u2:User),(u1)-[r:`follows`]->(u2)  WHERE u1.Id='"+userId+"' AND u2.Id='"+userToFollowId+"' DELETE r";

        try{
            IResultCursor cursor = await session.RunAsync(query);
            await cursor.ConsumeAsync();
        }

        finally
        {
            await session.CloseAsync();
        }
    }
}
