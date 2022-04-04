using ProjetoFinal.Models;

namespace ProjetoFinal.Data
{
    public static class FishDbInitializer
    {
        public static void InsertData(FishContext context)
        {
            var user = new User()
            {
                Username = "dory",
                FirstName = "Dory",
                LastName = "Blue",
                Email = "dont_forget@mail.com",
                Password = "easypassword",
                Gender = "Female",
                Mobile = 937772380,
                ProfileImage = "dory.jpg"
            };
            context.Users.Add(user);

            context.Publications.Add(new Publication
            {
                Text = "Hi, I'm Dory! Wait, what I was about to type?",
                User = user,
                Username = "dory",
            });
            context.SaveChanges();
        }
    }
}
