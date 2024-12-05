using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IRoomRepository Room { get; }

        IRoomPackageRepository RoomPackage { get; }
        IApplicationUserRepository User { get; }
        IRoomSetupRepository RoomSetup { get; }
        IServiceRepository Service { get; }
        IAmenityRepository Amenity { get; }
        IBookingRepository Booking { get; }
        ICompanyRepository Company { get; }

        void Save();
    }
}
