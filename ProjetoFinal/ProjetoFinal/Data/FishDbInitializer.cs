using ProjetoFinal.Models;

namespace ProjetoFinal.Data
{
    public static class FishDbInitializer
    {
        public static void InsertData(FishContext context)
        {
            var user = new User()
            {
                FirstName = "Pedro",
                LastName = "Santos",
                Email = "pedrosantos@gmail.com"
            };
            context.Users.Add(user);

            context.Publications.Add(new Publication
            {
                Text = "Este é a primeira publicação no Fish",
                User = user
            });
            context.SaveChanges();
        }
    }
}
