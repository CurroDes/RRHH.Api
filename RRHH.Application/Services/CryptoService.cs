using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Services;

public class CryptoService
{
    private readonly IConfiguration _configuration;

    public CryptoService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string DecryptPassword(string encryptedPassword, string key)
    {
        UtilService util = new UtilService(Encoding.ASCII.GetBytes(key));
        return util.DesCifrar(encryptedPassword, _configuration["ClaveCifrado"]);
    }
}
