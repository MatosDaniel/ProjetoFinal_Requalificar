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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Edit(int id, Publication publication)
        {
            throw new NotImplementedException();
        }
    }
}
