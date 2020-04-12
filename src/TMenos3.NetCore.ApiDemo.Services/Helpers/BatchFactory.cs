namespace TMenos3.NetCore.ApiDemo.Services.Helpers
{
    internal class BatchFactory : IBatchFactory
    {
        public Batch<TInput, TOutput> Create<TInput, TOutput>(int size) =>
            new Batch<TInput, TOutput>(size);
    }
}
