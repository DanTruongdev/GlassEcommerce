namespace GlassECommerce.Common
{
    public static class OrderStatus
    {
        public static new List<string> ValidStatus = new List<string>(){ "Pending", "Processing", "Delivering", "Delivered", "Canceled" };
        public static string PENDING = "Pending";
        public static string PROCESSING = "Processing";
        public static string DELIVERING = "Delivering";
        public static string DELIVERED = "Delivered";
        public static string CANCELED = "Canceled";
    }
}
