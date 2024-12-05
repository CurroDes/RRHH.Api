using Microsoft.Extensions.Configuration;
using RRHH.Application.DTOs;
using RRHH.Application.Services;
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Mapper;

public class RrhhMapper
{
    private readonly IConfiguration _configuration;
    public RrhhMapper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Rrhh MaptoRrhh(RrhhDTO r)
    {
        var claveCifrado = _configuration["ClaveCifrado"];
        byte[] keyByte = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        UtilService util = new UtilService(keyByte);

        return new Rrhh
        {
            Name = r.Name,
            Email = r.Email,
            Password = Encoding.ASCII.GetBytes(util.Cifrar(r.PassWord, claveCifrado)),
            DepartmentId = r.DepartmentId
        };
    }
}
