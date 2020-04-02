using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForumApp.Data.Helpers.Reflection
{
    static class ReflectionHelper
    {
        public static IEnumerable<PropertyWrapper> GetPropertiesAndValues<T>(this T obj)
        {
            return obj.GetType().GetProperties().Select(p =>
                        new PropertyWrapper()
                        {
                            Name = p.Name,
                            Value = p.GetValue(obj),
                            Type = p.PropertyType
                        });
        }
        public static bool DoesImplementGeneric(this Type type, Type genericInterface)
        {
            return type.GetInterfaces()
                       .Any(@interface => @interface.IsGenericType
                       && @interface.GetGenericTypeDefinition() == genericInterface);
        }
    }
}
