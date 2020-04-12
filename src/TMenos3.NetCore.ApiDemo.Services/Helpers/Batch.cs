using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMenos3.NetCore.ApiDemo.Services.Helpers
{
    internal class Batch<TInput, TOutput>
    {
        private IEnumerable<TInput> _list;
        private Func<TInput, Task<TOutput>> _asyncTask;
        private readonly int _size;

        public Batch(int size) => _size = size;

        public Batch<TInput, TOutput> ForList(IEnumerable<TInput> list)
        {
            _list = list;
            return this;
        }

        public Batch<TInput, TOutput> Apply(Func<TInput, Task<TOutput>> asyncTask)
        {
            _asyncTask = asyncTask;
            return this;
        }

        public async Task<IEnumerable<TOutput>> ExecuteAsync()
        {
            var result = new List<TOutput>();

            foreach (var batch in GetBatches())
            {
                var tasks = batch.Select(_asyncTask);
                var batchResult = await Task.WhenAll(tasks);
                result.AddRange(batchResult);
            }

            return result;
        }

        private IEnumerable<IEnumerable<TInput>> GetBatches() =>
            _list.ChunkBy(_size);
    }
}
