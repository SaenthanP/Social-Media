using AuthenticationService.Dtos;

namespace AuthenticationService.Data{
    public class UserRepo : IUserRepo
    {
        private readonly UserContext _context;

        public UserRepo(UserContext context)
        {
            _context=context;
        }
        public ReadUserDto CreateUser(CreateUserDto createUserDto)
        {
            throw new System.NotImplementedException();
        }

        public bool IsUsernameExists(string username)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }
    }
}