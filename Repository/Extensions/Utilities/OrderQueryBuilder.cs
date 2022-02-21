using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Repository.Extensions.Utilities
{
    public static class OrderQueryBuilder
    {
        public static string CreateOrderQueryBuilder<T>(string OrderByQueryString)
        {
            var orderParams = OrderByQueryString.Split(",");

            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                var propertyField = param.Split(" ")[0];

                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyField, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null) continue;

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name} {direction},");
            }

            return orderQueryBuilder.ToString().TrimEnd(',', ' ');
        }
    }
}
