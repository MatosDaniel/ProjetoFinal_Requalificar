﻿using ProjetoFinal.Models;

namespace ProjetoFinal.Service
{
    public interface IUserService
    {
        public abstract User GetById(int id);
        public abstract User Get(string email, string password);
        public abstract User Create(User user);
        public abstract void Delete(User user);
        public abstract void Edit(int id, User user);
        public abstract User FindByEmail(string email);
    }
}
