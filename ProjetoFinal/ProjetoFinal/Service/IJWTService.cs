﻿using ProjetoFinal.Models;

namespace ProjetoFinal.Service
{
    public interface IJWTService
    {
        string GenerateToken(string key, string issuer, string audience, User user);
        bool IsTokenValid(string key, string issuer, string audience, string token);
    }
}