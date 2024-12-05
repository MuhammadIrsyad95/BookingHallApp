using OnePointBooking.Application.Common.Interfaces;
using OnePointBooking.Domain.Entities;
using OnePointBooking.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Infrastructure.Respository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IRoomRepository Room { get; private set; }
        public IRoomPackageRepository RoomPackage { get; private set; }
        public IApplicationUserRepository User { get; private set; }
        public IRoomSetupRepository RoomSetup { get; private set; }
        public IServiceRepository Service { get; private set; }

        public IAmenityRepository Amenity { get; private set; } 
        public IBookingRepository Booking { get; private set; } 
        public ICompanyRepository Company { get; private set; } 

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Room = new RoomRepository(_db);
            User = new ApplicationUserRepository(_db);
            RoomPackage = new RoomPackageRepository(_db);
            Amenity = new AmenityRepository(_db);
            RoomSetup =new RoomSetupRepository(_db);
            Booking = new BookingRepository(_db);
            Company = new CompanyRepository(_db);
            Service = new ServiceRepository(_db);

        }
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
