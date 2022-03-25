using ProjetoFinal.Models;

namespace ProjetoFinal.Service
{
    public class UserService : IUserService
    {
        private readonly FishContext context;

        public UserService(FishContext context)
        {
            this.context = context;
        }

        public User Create(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public void Edit(int id, User user)
        {
            throw new NotImplementedException();
        }

        public User? FindByEmail(string email)
        {
            return context.Users.FirstOrDefault(x => x.Email == email);
        }

        public User Get(string email, string password)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
