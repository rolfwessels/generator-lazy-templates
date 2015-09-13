using System;
using System.Linq;
using System.Reflection;

namespace MainSolutionTemplate.Utilities.Helpers
{
    public class MyReflectionHelper
    {
        public static Type FindOfType(Assembly ns, string typeName)
        {
            return ns.GetTypes().FirstOrDefault(x => x.Name == typeName);
        }

        public static Type MakeGenericType(Type type, Type reflectionhelpertest)
        {
            return type.MakeGenericType(reflectionhelpertest);
        }
    }
}