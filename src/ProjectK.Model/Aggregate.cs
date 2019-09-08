using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ProjectK.Core.Aggregates;
using ProjectK.Core.Helpers;

namespace ProjectK.Model
{
    public abstract class Aggregate<T> : IAggregate where T: IAggregate
    {
        private readonly ExpressionHelper _expressionHelper  = new ExpressionHelper();

        public Guid Id { get; set; }
        public int Version { get; set; }
        
        public bool SetProperty(Expression<Func<T, object>> expression, T incoming)
        {
            if (!PropertyProvided(expression, incoming)) return false;

            var prop = GetProperty(expression, incoming);
            var value = prop.GetValue(incoming);
            prop.SetValue(this, value);

            return true;
        }

        private PropertyInfo GetProperty(Expression expression, T instance)
        {
            var name = _expressionHelper.GetName(expression);
            var properties = instance.GetType().GetProperties();

            return properties.FirstOrDefault(p => p.Name == name);
        }

        protected bool PropertyProvided(Expression expression, T instance)
        {
            var prop = GetProperty(expression, instance);

            if (prop == null) return false;

            var value = prop.GetValue(instance);
            var propType = prop.PropertyType;

            var defaultInstance = GetDefaultInstance(propType);

            if (value != null && defaultInstance != null)
            {
                return !value.Equals(defaultInstance);
            }

            return value != null;
        }

        private object GetDefaultInstance(Type propType)
        {
            object defaultInstance;
            if (propType.Name == nameof(String)) defaultInstance = null;
            else if (propType.Name == nameof(Int32)) defaultInstance = 0;
            else if (propType.Name == nameof(DateTime)) defaultInstance = DateTime.MinValue;
            else if (propType.Name == nameof(Guid)) defaultInstance = Guid.Empty;
            else defaultInstance = Activator.CreateInstance(propType);

            return defaultInstance;
        }
    }
}
