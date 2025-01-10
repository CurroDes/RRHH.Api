using RRHH.Application.DTOs;
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Mapper;

public class ReviewsMapper
{
    public PerformanceReview MapToReview(PerformanceReviewsDTO p, PerformanceReview pr)
    {
        pr.EmployeeId = p.EmployeeId;
        pr.ReviewDate = DateTime.Now;
        pr.Score = p.Score;
        pr.Comments = p.Comments;

        return pr;
    }
}
