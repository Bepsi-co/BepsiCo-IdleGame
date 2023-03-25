using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Helpers
{
    public static class Reflection
    {
        public static bool IsSameOrSubclass(Type query, Type potentialBase)
        {
            return query.IsSubclassOf(potentialBase)
                   || query == potentialBase;
        }
    }
}
