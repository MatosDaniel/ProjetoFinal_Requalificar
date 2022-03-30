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

        public User GetByUsername(User user)
        {
            return context.Users.FirstOrDefault(u => u.Username == user.Username);
        }

        public void Delete(User user)
        {
            if (user is not null)
            {
                context.Users.Remove(user);
                context.SaveChanges();

            }
            else
            {
                throw new NullReferenceException("User not found");
            }
        }

        public User Edit(int id, User user)
        {
            var userToUpdate = context.Users.Find(id);
            if (userToUpdate is null)
            {
                throw new NullReferenceException("User does not exist");
            }
            else
            {
                userToUpdate.Username = user.Username;
                userToUpdate.Password = user.Password;
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.Gender = user.Gender;
                userToUpdate.Mobile = user.Mobile;
                userToUpdate.Email = user.Email;

                context.SaveChanges();

                return userToUpdate;
            }
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
            var user = context.Users.FirstOrDefault(x => x.UserId == id);
            return user;
        }
        
        public void UpdateImage(int id, string profileImage)
        {
            var imageUpdate = context.Users.Find(id);
            imageUpdate.ProfileImage = profileImage;
            context.SaveChanges();
        }
    }
}
