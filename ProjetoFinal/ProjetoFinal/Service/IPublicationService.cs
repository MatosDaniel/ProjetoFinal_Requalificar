using ProjetoFinal.Models;

namespace ProjetoFinal.Service
{
    //Interface for all the services related to the publication
    public interface IPublicationService
    {
        public abstract IEnumerable<Publication> GetAll();
        public abstract Publication GetById(int id);
        public abstract IEnumerable<Publication> GetPostById(int id);
        public abstract Publication Create(Publication publication);
        public abstract void Delete(int idPub);
        public abstract void EditPublication(int id, Publication publication);
        public abstract void Likes(int id);
    }
}
