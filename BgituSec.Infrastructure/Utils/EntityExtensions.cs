using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
