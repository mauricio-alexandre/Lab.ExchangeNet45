using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Application.Tests.Utils
{
    internal static class ObjectExtensions
    {
        public static bool PublicInstancePropertiesEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self == null || to == null) return self == to;

            Type type = typeof(T);
            IEnumerable<string> ignoreList = new List<string>(ignore);

            IEnumerable<object> unequalProperties =
                from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where !ignoreList.Contains(pi.Name) && pi.GetUnderlyingType().IsSimpleType() && pi.GetIndexParameters().Length == 0
                let selfValue = type.GetProperty(pi.Name)?.GetValue(self, null)
                let toValue = type.GetProperty(pi.Name)?.GetValue(to, null)
                where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
                select selfValue;

            return !unequalProperties.Any();
        }
    }
}
