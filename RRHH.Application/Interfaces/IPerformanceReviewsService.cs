using RRHH.Application.DTOs;
using RRHH.Application.Services;
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Interfaces
{
    public interface IPerformanceReviewsService
    {
        Task<Result> PostReviewsService(int id, PerformanceReviewsDTO p);
    }
}
