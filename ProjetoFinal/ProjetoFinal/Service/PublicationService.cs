﻿using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Publication> GetPostById(int id)
        {
            //var pub = context.Publications.Find(u => u.User.UserId == id);

            var pub = context.Publications.Include(c => c.User);
            var userpub = pub.Where(c => c.User.UserId == id);
            return userpub;

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

        public IEnumerable<Publication> GetByUser(int id)
        {
            var allPosts = GetAll();

            List<Publication> postsByUser = new List<Publication>();

            foreach(var posts in allPosts)
            {
                //var teste = posts;

                if (posts.User.UserId == id)
                {
                    postsByUser.Add(posts);
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            if(postsByUser.Count == 0)
            {
                Publication newpost = new Publication();
                newpost.Text = "You haven't post anything";
                postsByUser.Add(newpost);
            }
            return postsByUser;
        }
    }
}
