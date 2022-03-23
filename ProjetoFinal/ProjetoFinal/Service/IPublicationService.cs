using ProjetoFinal.Models;

namespace ProjetoFinal.Service
{
    public interface IPublicationService
    {
        public abstract IEnumerable<Publication> GetAll();
        public abstract Publication GetById(int id);
        public abstract Publication Create(Publication publication);
        public abstract void Delete(Publication publication);
        public abstract void Edit(int id, Publication publication);
    }
}
