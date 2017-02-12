using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Methods
{
    public static class ObjectType
    {
        public static string Name<T>(this T value) => Enum.GetName(typeof(T), value);
    }
}
