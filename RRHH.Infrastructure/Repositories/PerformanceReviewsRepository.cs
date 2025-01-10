using Microsoft.EntityFrameworkCore;
using RRHH.Domain.Data;
using RRHH.Domain.Entities;
using RRHH.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Infrastructure.Repositories
{
    public class PerformanceReviewsRepository<T> : IPerformanceReviewsRepository<T> where T : PerformanceReview
    {
        private readonly ApprrhhApiContext _context;
        public PerformanceReviewsRepository(ApprrhhApiContext context)
        {
            _context = context;
        }

        public async Task ModifyReviews(T reviews)
        {
            _context.Entry(reviews).State = EntityState.Modified;
        }

        public async Task AddReviewAsync(T reviews)
        {
            await _context.AddAsync(reviews);
        }

    }
}
