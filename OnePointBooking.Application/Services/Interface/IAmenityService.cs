﻿using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Services.Interface
{
    public interface IAmenityService
    {
        IEnumerable<Amenity> GetAllAmenities();
        Amenity GetAmenityById(int id);
        void CreateAmenity(Amenity amenity);
        void UpdateAmenity(Amenity amenity);
        bool DeleteAmenity(int id);

    }
}
