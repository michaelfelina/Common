namespace Common.Methods.Extensions
{
    public static class ExtensionsNumeric
    {
        public static string ToCurrency(this double value) => $"{value:#,##0.00}";
        public static bool ToBoolean(this int value)
        {
            bool result;
            try
            {
                if (value == 1) result = true;
                else result = false;
            }
            catch
            {
                result = false;
            }

            return result;
        }

    }
}
