using ProjetoFinal.Models;

namespace ProjetoFinal.Service
{
    //Interface for all the services related to the user
    public interface IUserService
    {
        public abstract User GetById(int id);
        public abstract User Get(string email, string password);
        public abstract User Create(User user);
        public abstract void Delete(User user);
        public abstract User Edit(int id, User user);
        public abstract User GetByUsername(User user);
        public abstract User FindByEmail(string email);
        public abstract void UpdateImage(int id, string profileImage);
    }
}
