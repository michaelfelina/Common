namespace Common.Methods.Enumumerators
{
    public enum Status
    {
        Success = 0,
        Failed = 1,
    }

    public static class StatusEx
    {
        //public static string ToString(this Status value) => System.Enum.GetName(typeof(Status), value);
        public static int ToInt(this Status value) => (int) value;
    }
}

