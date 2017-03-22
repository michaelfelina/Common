using System;
using System.Globalization;

namespace Common.Methods
{
    public static class ExtensionsString
    {
        public static ulong HexToDecimal(this string value)
            => ulong.Parse(value, System.Globalization.NumberStyles.HexNumber);

        public static double ToDouble(this string value)
        {
            double result = 0;
            double.TryParse(value, out result);
            return result;
        }

        public static int ToInt(this string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }
        
        public static bool IsNumeric(this string value)
        {
            double result;
            return double.TryParse(value, out result);
        }

        public static bool ToBoolean(this string value)
        {
            bool result;
            try
            {
                result = bool.Parse(value);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static DateTime? StringToNullableTime(this string timeString)
        {
            DateTime? result;
            timeString = DateTime.Now.ToShortDateString() + " " + timeString;
            try
            {
                result = DateTime.Parse(timeString);
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        public static DateTime? StringToNullableDate(this string dateString)
        {
            DateTime? result;
            try
            {
                result = DateTime.ParseExact(dateString,"MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                result = null;
            }
            
            return result;
        }

        public static DateTime? StringToNullableDateTime(this object dateString)
        {
            DateTime? result;
            try
            {
                result = DateTime.ParseExact(dateString.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        public static DateTime StringToTime(this string timeString)
        {
            timeString = DateTime.Now.ToShortDateString() + " " + timeString;
            var result = DateTime.Parse(timeString);
            return result;
        }

        public static DateTime StringToDate(this string dateString)
            => DateTime.ParseExact(dateString, "MM/dd/yyyy", CultureInfo.InvariantCulture);

        public static DateTime StringToDateTime(this string dateString)
            => DateTime.ParseExact(dateString, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);


        public static bool IsDate(this string anyString)
        {
            try
            {
                var dummyDate = DateTime.Parse(anyString);
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}
