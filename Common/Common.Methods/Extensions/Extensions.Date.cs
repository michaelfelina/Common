﻿using System;

namespace Common.Methods.Extensions
{
    public static class Extensions
    {
        public static string ConvertToString(this DateTime? date)
        {
            if (date != null) return date.ToString();
            return null;
        }
    }
}
