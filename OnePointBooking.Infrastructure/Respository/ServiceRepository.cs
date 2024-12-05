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
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _db;

        public ServiceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Service entity)
        {
            _db.Services.Update(entity);
        }
        public IEnumerable<Service> GetServicesByRoomId(int roomId)
        {
            return _db.Services
                       .Where(rs => rs.RoomId == roomId)
                       .ToList();
        }
    }
}
