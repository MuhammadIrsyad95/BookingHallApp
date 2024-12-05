using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Services.Interface
{
    public interface ICompanyService
    {
        IEnumerable<Company> GetAllCompanies();
        Company GetCompanyById(int id);
        void CreateCompany(Company company);
        void UpdateCompany(Company company);
        bool DeleteCompany(int id);

       
    }
}
