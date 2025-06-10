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
    }
}
