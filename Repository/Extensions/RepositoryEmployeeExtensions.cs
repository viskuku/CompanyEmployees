using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utilities;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge) =>
            employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));

        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return employees;

            var lowerCaseTerm = searchTerm.Trim()?.ToLower();

            return employees.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        // ?orderby=name,age desc
        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string OrderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(OrderByQueryString)) return employees.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQueryBuilder<Employee>(OrderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery)) return employees.OrderBy(e => e.Name);

            return employees.OrderBy(orderQuery);
        }

    }
}
