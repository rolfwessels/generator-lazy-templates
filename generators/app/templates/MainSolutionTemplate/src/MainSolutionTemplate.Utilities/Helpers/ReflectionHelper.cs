using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MainSolutionTemplate.Utilities.Helpers
{
    public class ReflectionHelper
    {
        public static Type FindOfType(Assembly ns, string typeName)
        {
            return ns.GetTypes().FirstOrDefault(x => x.Name == typeName);
        }

        public static Type MakeGenericType(Type type, Type reflectionhelpertest)
        {
            return type.MakeGenericType(reflectionhelpertest);
        }

       
        public static PropertyInfo GetPropertyInfo<TSource, TType>(Expression<Func<TSource, TType>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.",propertyLambda));

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.",propertyLambda));

            return propInfo;
        }

        public static string GetPropertyString<T, TType>(Expression<Func<T, TType>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda));

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", propertyLambda));
            var name = propInfo.Name;
            var e = member.Expression as MemberExpression;
            while (e !=null)
            {
                name = e.Member.Name + '.' + name;
                e = e.Expression as MemberExpression;
            }

            return name;
        }
    }
}