using FinancialHub.Common.Extensions;
using System.Text;

namespace FinancialHub.Core.Infra.Caching.Extensions
{
    internal static class ObjectExtensions
    {
        internal static byte[] ToByteArray(this object obj)
        {
            return Encoding.UTF8.GetBytes(obj.ToJson());
        }

        internal static T? FromByteArray<T>(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes).FromJson<T>();
        }
    }
}
