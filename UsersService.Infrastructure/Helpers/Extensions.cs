using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace UsersService.Infrastructure.Helpers
{
    public static class Extensions
    {
        public static string GetEnumDisplayName(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DisplayAttribute[] attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].GetName();
            else
                return value.ToString();
        }
        public static int Getrandomnumber()
        {
            Random generator = new Random();
            int rotp = generator.Next(100000, 999999);
            return rotp;  
        }
    }
}
