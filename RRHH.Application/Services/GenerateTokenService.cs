using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RRHH.Application.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Services;

public class GenerateTokenService
{

    private readonly IConfiguration _configuration;
    public GenerateTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(AuthApiViewModelDTO userInfo)
    {
        // Clave secreta para firmar el token
        var _symmetricSecurityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:ClaveSecreta"]));

        var _signingCredentials = new SigningCredentials(
            _symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        // Cabecera del token
        var _Header = new JwtHeader(_signingCredentials);

        // Claims: datos asociados al token
        var _Claims = new[] {
        new Claim(JwtRegisteredClaimNames.Email, userInfo.email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID único para cada token
        new Claim(JwtRegisteredClaimNames.Iat,
                  new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64) // Tiempo de emisión (issued at)
    };

        // Cargar la información del payload (issuer, audiencia, etc.)
        var _Payload = new JwtPayload(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: _Claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(24) // Token válido por 24 horas
        );

        // Crear el token
        var _Token = new JwtSecurityToken(_Header, _Payload);

        // Convertir el token en formato string
        string token = new JwtSecurityTokenHandler().WriteToken(_Token);

        return token; // Retornar el token generado
    }
}

