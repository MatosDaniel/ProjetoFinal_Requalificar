using ProjetoFinal.Models;

namespace ProjetoFinal.Data
{
    public static class FishDbInitializer
    {
        public static void InsertData(FishContext context)
        {
            var user = new User()
            {
                Username = "Pedrinho",
                FirstName = "Pedro",
                LastName = "Santos",
                Email = "pedrosantos@gmail.com",
                Password = "batatasfritas",
                Gender = "Male",
                Mobile = 937772380,
                ProfileImage = "default.png"
            };
            context.Users.Add(user);

            context.Publications.Add(new Publication
            {
                Text = "Este é a primeira publicação no Fish",
                User = user,
                Username = "Pedrinho",
            });
            context.SaveChanges();
        }
    }
}
