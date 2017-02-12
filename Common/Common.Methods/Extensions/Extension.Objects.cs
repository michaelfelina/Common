using System;
using System.Globalization;

namespace Common.Methods.Extensions
{
    public static class ExtensionsObject
    {
        public static int ToInt(this object obj)
        {
            int result;
            int.TryParse(obj.ToString(), out result);
            return result;
        }

        public static double ToDouble(this object value)
        {
            double result;
            double.TryParse(value.ToString(), out result);
            return result;
        }

        public static bool ToBoolean(this object value)
        {
            bool result;
            if (value != null && (value.ToString().ToLower() == "true" || value.ToString() == "1"))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        } 

        public static DateTime? ObjectToNullableTime(this object timeString)
        {
            DateTime? result;
            timeString = DateTime.Now.ToShortDateString() + " " + timeString;
            try
            {
                result = DateTime.Parse(timeString.ToString());
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        public static DateTime? ObjectToNullableDate(this object dateString)
        {
            DateTime? result;
            try
            {
                result = DateTime.Parse(dateString.ToString());
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        public static DateTime? ObjectToNullableDateTime(this object dateString)
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

        public static DateTime ObjectToTime(this object timeString)
        {
            timeString = DateTime.Now.ToShortDateString() + " " + timeString;
            var result = DateTime.Parse(timeString.ToString());
            return result;
        }

        public static DateTime ObjectToDate(this object dateString) => DateTime.Parse(dateString.ToString());
        

        public static DateTime ObjectToDateTime(this object dateString) => DateTime.Parse(dateString.ToString());

        public static string ToNullableString(this object value)
        {
            return string.IsNullOrWhiteSpace(value.ToString()) ? null : value.ToString();
        }
    }
}
