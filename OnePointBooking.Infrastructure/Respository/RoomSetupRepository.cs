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
    public class RoomSetupRepository : Repository<RoomSetup>, IRoomSetupRepository
    {
        private readonly ApplicationDbContext _db;

        public RoomSetupRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(RoomSetup entity)
        {
            _db.RoomSetups.Update(entity);
        }
        public IEnumerable<RoomSetup> GetRoomSetupsByRoomId(int roomId)
        {
            return _db.RoomSetups
                       .Where(rs => rs.RoomId == roomId)
                       .ToList();
        }
    }
}
