using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Domain.Interfaces
{
    public interface IPerformanceReviewsRepository<T> where T : PerformanceReview
    {
        Task ModifyReviews(T reviews);
        Task AddReviewAsync(T reviews);
    }
}
