using System;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using PersonalNetworkService.Models;
using PersonalNetworkService.Services;
using System.Threading.Tasks;
using Neo4jClient;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

public class UserNetworkService : IUserNetworkService
{

    private readonly IConfiguration _configuration;
    private readonly IDriver _driver;
    private readonly IMapper _mapper;

    public UserNetworkService(IConfiguration configuration,IDriver driver, IServiceProvider serviceProvider)
    {   
        _configuration=configuration;
        _driver=driver;
        using (var scope = serviceProvider.CreateScope()){
             _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        }
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

    public async Task<User> GetUser(string id)
    {
        var session=_driver.AsyncSession();
        var query="MATCH (u1:User) WHERE u1.Id='"+id+"' RETURN u1";

        try{
            IResultCursor cursor = await session.RunAsync(query);
            await cursor.FetchAsync();
            var result=cursor.Current.Values.Values.FirstOrDefault();
            
            if(result!=null){
                var user=new User();
                var dic=result.As<INode>().Properties;
                 foreach(var item in dic){
                     if(item.Key=="Id"){
                         user.UserId=item.Value.ToString();
                     }

                    if(item.Key=="Username"){
                         user.Username=item.Value.ToString();
                     }
                }
                return user;

            }
            
        } finally{
            await session.CloseAsync();
        }

        return null;
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
