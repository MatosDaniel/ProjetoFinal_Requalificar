using ProjetoFinal.Models;

namespace ProjetoFinal.Data
{
    public static class FishDbInitializer
    {
        public static void InsertData(FishContext context)
        {
            var user = new User()
            {
                Username = "psantos",
                FirstName = "Pedro",
                LastName = "Santos",
                Email = "pedrosantos@gmail.com",
                Password = "password",
                Gender = "Male",
                Mobile = 937772380,
                ProfileImage = "default.jpg"
            };
            context.Users.Add(user);

            context.Publications.Add(new Publication
            {
                Text = "This is my first Gluglu!",
                User = user,
                Username = "psantos",
            });
            context.SaveChanges();
        }
    }
}
