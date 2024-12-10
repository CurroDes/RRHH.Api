using RRHH.Application.Interfaces;
using RRHH.Domain.Entities;
using RRHH.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Services;

public class AuthenticationService
{

    private IEmployeesRepository<Employee> _employeesRepository;
    private CryptoService _cryptoService;
    public AuthenticationService(IEmployeesRepository<Employee> employeesRepository, CryptoService cryptoService)
    {
        _employeesRepository = employeesRepository;
        _cryptoService = cryptoService;
    }

    public async Task<Employee> AuthenticationAsync(string email, string password)
    {
        var employee = await _employeesRepository.GetEmployeesEmailAsync(email);
        if (employee == null || password != _cryptoService.DecryptPassword(Encoding.ASCII.GetString(employee.Password), "clave_de_cifrado"))
        {
            throw new Exception("Credenciales incorrectas");
        }

        return employee;
    }
}
