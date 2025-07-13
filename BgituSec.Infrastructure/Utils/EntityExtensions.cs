using System.Globalization;
using System.Reflection;

namespace BgituSec.Infrastructure.Utils
{
    public static class EntityExtensions
    {
        public static void CopyChanges<T>(this T target, T source)
        {
            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var p in props)
            {
                var newValue = p.GetValue(source);
                if (newValue != null)
                {
                    p.SetValue(target, newValue);
                }
            }
        }
        public static (int Width, int Height) ParseIntSize(string size)
        {
            var culture = new CultureInfo("ru-RU");
            var parts = size.Split(';');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }
        public static (double Width, double Height) ParseDoubleSize(string size)
        {
            var culture = new CultureInfo("ru-RU");
            var parts = size.Split(';');
            return (double.Parse(parts[0], culture), double.Parse(parts[1], culture));
        }
        public static (double x, double y) ParsePosition(string point)
        {
            var culture = new CultureInfo("ru-RU");
            var parts = point.Split(';');
            return (double.Parse(parts[0], culture), double.Parse(parts[1], culture));
        }
    }
}
