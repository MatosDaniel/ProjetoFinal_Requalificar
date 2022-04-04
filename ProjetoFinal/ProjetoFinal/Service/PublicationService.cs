using Microsoft.EntityFrameworkCore;
using ProjetoFinal.Models;

namespace ProjetoFinal.Service
{
    //Implementation of the interface for the publication services
    public class PublicationService : IPublicationService
    {
        private readonly FishContext context;

        public PublicationService(FishContext context)
        {
            this.context = context;
        }   

        //Service that creates a new post
        public Publication Create(Publication publication)
        {            
            context.Publications.Add(publication);
            context.SaveChanges();
            return publication;
            
        }

        //Service that gets a single post by the post Id
        public Publication GetById(int id)
        {
            var pub = context.Publications.Include(u => u.User).SingleOrDefault(b => b.IdPub == id);
            return pub;

        }

        //Service that gets a list of post published by a single user
        public IEnumerable<Publication> GetPostById(int id)
        {
            var pub = context.Publications.Include(c => c.User);
            var userpub = pub.Where(c => c.User.UserId == id);
            return userpub;
        }

        //Service that deletes a post by the post Id
        public void Delete(int id)
        {
            var pub = GetById(id);

            if (pub is not null)
            {
                context.Publications.Remove(pub);
                context.SaveChanges();

            }

            else
            {
                throw new NullReferenceException("Publication not found");
            }
        }

        //Service that adds a like to a post 
        public void Likes(int id)
        {
            var pub = GetById(id);

            if (pub is not null)
            {
                pub.Likes++;
                context.SaveChanges();
            }

            else
            {
                throw new NullReferenceException("Publication not found");
            }
        }
        
        //Service that gets a List of all the posts in the database
        public IEnumerable<Publication> GetAll()
        {
            return context.Publications;
        }

        //Service that edits a post 
        public void EditPublication(int id, Publication publication)
        {
            var post = GetById(id);

            if(post != null)
            {
                post.Text = publication.Text;
                context.SaveChanges();
            }

            else
            {
                throw new NullReferenceException("Publication not found!");
            }
        }
    }
}
