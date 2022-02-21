using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Repository.DataShaping
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties { get; set; }
        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            var properties = GetRequiredProperties(fieldsString);

            return FetchData(entities, properties);
        }

        public ShapedEntity ShapeData(T entity, string fieldsString)
        {
            var properties = GetRequiredProperties(fieldsString);

            return FetchDataForEntity(entity, properties);
        }


        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            var fieldProperty = fieldsString?.Trim().Split(",", StringSplitOptions.RemoveEmptyEntries);

            if (fieldsString == null) return Properties;

            var propInfoList = new List<PropertyInfo>();

            foreach (var param in fieldProperty)
            {
                var objectProperty = Properties.FirstOrDefault(p => p.Name.Equals(param, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null) continue;

                propInfoList.Add(objectProperty);
            }

            return propInfoList;
        }

        private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> propertyInfos)
        {
            var fetchDataEntities = new List<ShapedEntity>();

            foreach (var entity in entities)
            {
                var fetchDataForEntity = FetchDataForEntity(entity, propertyInfos);

                fetchDataEntities.Add(fetchDataForEntity);
            }

            return fetchDataEntities;
        }

        private ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> propertyInfos)
        {
            var expandObject = new ShapedEntity();

            foreach (var param in propertyInfos)
            {
                var objectPropertyValue = param.GetValue(entity);
                expandObject.Entity.TryAdd(param.Name, objectPropertyValue);
            }

            var objectProperty = entity.GetType().GetProperty("Id");
            expandObject.Id = (Guid)objectProperty.GetValue(entity);

            return expandObject;
        }
    }
}
