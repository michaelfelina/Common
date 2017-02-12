using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Methods.Extensions
{
    public static class ExtensionsBoolean
    {
        public static int ToInt(this bool value)
        {
            int result;
            try
            {
                result = value ? 1 : 0;
            }
            catch
            {
                result = 0;
            }

            return result;
        }
    }
}
