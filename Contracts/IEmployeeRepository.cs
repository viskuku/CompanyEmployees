using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmployeeRepository
    {

        Task<PagedList<Employee>> GetEmployees(Guid id, EmployeeParameters employeeParameters, bool trackChanges);

        Task<IEnumerable<Employee>> GetEmployees(Guid id, bool trackChanges);
        Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);

        void CreateEmployeeForCompany(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
