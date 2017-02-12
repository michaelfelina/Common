using System;

namespace Common.Methods
{
    public static class GenerateGuid
    {
        public static string Get() => Guid.NewGuid().ToString();
    }
}
