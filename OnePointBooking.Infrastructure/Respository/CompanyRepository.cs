using OnePointBooking.Application.Common.Interfaces;
using OnePointBooking.Domain.Entities;
using OnePointBooking.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Infrastructure.Respository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Company entity)
        {
            _db.Companies.Update(entity);
        }
    }
}
