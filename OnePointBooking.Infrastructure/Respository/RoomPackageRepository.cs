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
    public class RoomPackageRepository : Repository<RoomPackage>, IRoomPackageRepository
    {
        private readonly ApplicationDbContext _db;

        public RoomPackageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(RoomPackage entity)
        {
            _db.RoomPackages.Update(entity);
        }

        public IEnumerable<RoomPackage> GetRoomPackagesByRoomId(int roomId)
        {
            return _db.RoomPackages
                       .Where(rp => rp.RoomId == roomId)
                       .ToList();
        }
    }
}
