using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Helpers
{
    public static class Reflection
    {
        // returns true if query is a subclass of or the same type as potentialBase
        public static bool IsSameOrSubclass(Type query, Type potentialBase)
        {
            return query.IsSubclassOf(potentialBase)
                   || query == potentialBase;
        }

        // gets all types in assembly which are derived from type T
        public static IEnumerable<Type> FindAllDerivedFrom<T>() where T : class
        {
            return Assembly.GetAssembly(typeof(T))
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(T)));
        }
    }
}
