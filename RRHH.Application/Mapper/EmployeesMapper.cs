
using Microsoft.Extensions.Configuration;
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
    public class EmployeesMapper
    {
        private readonly IConfiguration _configuration;

        public EmployeesMapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Employee MapToEmployees(EmployeesDTO e)
        {
            var claveCifrado = _configuration["ClaveCifrado"];
            byte[] keyByte = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            UtilService util = new UtilService(keyByte);

            return new Employee
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                DepartmentId = e.DepartmentId,
                Role = e.Role,
                PhoneNumber = e.PhoneNumber,
                WorkingHoursId = e.WorkingHoursId,
                Password = Encoding.ASCII.GetBytes(util.Cifrar(e.Password, claveCifrado))
            };
        }

        public Employee MapToPutEmployees(EmployeesDTO e , Employee em)
        {
            var claveCifrado = _configuration["ClaveCifrado"];
            byte[] keyByte = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            UtilService util = new UtilService(keyByte);

            em.FirstName = e.FirstName;
            em.LastName = e.LastName;
            em.Email = e.Email;
            em.DepartmentId = e.DepartmentId;
            em.Role = e.Role;
            em.PhoneNumber = e.PhoneNumber;
            em.WorkingHoursId = e.WorkingHoursId;
            em.Password = Encoding.ASCII.GetBytes(util.Cifrar(e.Password, claveCifrado));


            return em;
        }
    }
}
