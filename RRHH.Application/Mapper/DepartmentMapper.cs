using RRHH.Api;
using RRHH.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Mapper
{
    public class DepartmentMapper
    {
        public DepartmentMapper()
        {

        }

        public Department MapToDepartment(DepartmentDTO d)
        {
            return new Department
            {
                DepartmentName = d.DepartmentName
            };
        }
    }
}
