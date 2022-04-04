using ProjetoFinal.Models;

namespace ProjetoFinal.Service
{
    //Implementation of the interface for the user services
    public class UserService : IUserService
    {
        private readonly FishContext context;

        public UserService(FishContext context)
        {
            this.context = context;
        }

        //Service that creates a new user
        public User Create(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        //Service that gets a single user by his username
        public User GetByUsername(User user)
        {
            return context.Users.FirstOrDefault(u => u.Username == user.Username);
        }

        //Service that deletes a user
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

        //Service that edits the details of a user
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

        //Service that gets a user by the email
        public User? FindByEmail(string email)
        {
            return context.Users.FirstOrDefault(x => x.Email == email);
        }

        //Service that gets a user by checking the email and password combination
        public User Get(string email, string password)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }

        //Service that gets a user by the id
        public User GetById(int id)
        {
            var user = context.Users.FirstOrDefault(x => x.UserId == id);
            return user;
        }
        
        //Service that updates the profile image
        public void UpdateImage(int id, string profileImage)
        {
            var imageUpdate = context.Users.Find(id);
            imageUpdate.ProfileImage = profileImage;
            context.SaveChanges();
        }
    }
}
