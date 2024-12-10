using RRHH.Application.DTOs;
using RRHH.Application.Services;
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Mapper
{
    public class TokensMapper
    {
        private readonly GenerateTokenService _generateTokenService;
        public TokensMapper(GenerateTokenService generateTokenService)
        {
             _generateTokenService = generateTokenService;
        }
        public Token MapToTokens(Token t, AuthApiViewModelDTO a, Employee e)
        {
            t.Token1 = _generateTokenService.GenerateToken(a);
            t.CreatedAtToken = DateTime.Now;
            t.ExpirationDate = DateTime.Now.AddHours(24);
            t.EmployeesId = e.Id;

            return t;
        }
    }
}
