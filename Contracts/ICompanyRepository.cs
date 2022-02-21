using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges);
        Task<Company> GetCompany(Guid guid, bool trackChanges);

        Task<IEnumerable<Company>> GetCompanyByIds(IEnumerable<Guid> companyIds, bool trackChanges);

        void CreateCompany(Company company);

        void DeleteCompany(Company company);
    }
}
