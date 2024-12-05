using Microsoft.AspNetCore.Hosting;
using OnePointBooking.Application.Common.Interfaces;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Services.Implementation
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            return _unitOfWork.Company.GetAll(); // Mengambil semua perusahaan dari unit of work
        }

        public Company GetCompanyById(int id)
        {
            return _unitOfWork.Company.Get(u => u.Id == id); // Mengambil perusahaan berdasarkan ID
        }

        public void CreateCompany(Company company)
        {
            _unitOfWork.Company.Add(company);
            _unitOfWork.Save();
        }

        public void UpdateCompany(Company company)
        {
            _unitOfWork.Company.Update(company);
            _unitOfWork.Save();
        }

        public bool DeleteCompany(int id)
        {
            try
            {
                Company objFromDb = _unitOfWork.Company.Get(u => u.Id == id);
                if (objFromDb != null)
                {
                    _unitOfWork.Company.Remove(objFromDb);
                    _unitOfWork.Save();
                    return true;
                }
                return false; // Perusahaan tidak ditemukan
            }
            catch (Exception)
            {
                return false; // Terjadi kesalahan saat menghapus
            }
        }

        // Metode lainnya sesuai kebutuhan
    }
}
