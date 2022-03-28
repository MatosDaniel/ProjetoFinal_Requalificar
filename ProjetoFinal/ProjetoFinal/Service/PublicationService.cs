using Microsoft.EntityFrameworkCore;
using ProjetoFinal.Models;

namespace ProjetoFinal.Service
{
    public class PublicationService : IPublicationService
    {
        private readonly FishContext context;

        public PublicationService(FishContext context)
        {
            this.context = context;
        }   

        public Publication Create(Publication publication)
        {
            context.Publications.Add(publication);
            context.SaveChanges();
            return publication;
            
        }

        public Publication GetById(int id)
        {
            var pub = context.Publications.Include(u => u.User).SingleOrDefault(b => b.IdPub == id);
            return pub;

        }

        public void Delete(Publication publication)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Publication> GetAll()
        {
            return context.Publications;
        }

        public void EditPublication(Publication publication)
        {
            if(publication != null)
            {
                context.Publications.Update(publication);
                context.SaveChanges();
            }
            else
            {
                throw new NullReferenceException("Publication not found!");
            }
        }
    }
}
