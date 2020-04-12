using System.Collections.Generic;
using System.Linq;

namespace TMenos3.NetCore.ApiDemo.Services.Helpers
{
    public static class BatchExtensions
    {
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(
            this IEnumerable<T> source,
            int chunkSize) =>
                source
                    .Select((x, i) => new { Index = i, Value = x })
                    .GroupBy(x => x.Index / chunkSize)
                    .Select(x => x.Select(v => v.Value));
    }
}
