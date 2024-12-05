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
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext _db;

        public RoomRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Room entity)
        {
            _db.Rooms.Update(entity);
        }
    }
}
