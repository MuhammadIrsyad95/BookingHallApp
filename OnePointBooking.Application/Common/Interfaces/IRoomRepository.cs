﻿using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Common.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        void Update(Room entity);
    }
}
