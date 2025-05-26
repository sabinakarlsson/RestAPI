using RestAPI.Services.Interfaces;
using RestAPI.ViewModel;


namespace RestAPI.Services
{
    public class UserService : IUserService
    {
        //private readonly BankDbContext _context;

        public static List<User> Users = new List<User>();

        public static User GetUser(string userName)
        {
            return Users.FirstOrDefault(u => u.UserName == userName);
        }
        public static void AddUser(User user)
        {
            Users.Add(user);
        }
        public static void RemoveUser(string userName)
        {
            var user = GetUser(userName);
            if (user != null)
            {
                Users.Remove(user);
            }
        }

    }
}
