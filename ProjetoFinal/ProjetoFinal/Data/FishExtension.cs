using ProjetoFinal.Models;

namespace ProjetoFinal.Data
{
    public static class FishExtension
    {
        public static void CreateDbIfNotExists(this IHost host)
        {
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<FishContext>();
                    if (context.Database.EnsureCreated())
                    {
                        FishDbInitializer.InsertData(context);
                    }
                }
            }
        }
    }
}
