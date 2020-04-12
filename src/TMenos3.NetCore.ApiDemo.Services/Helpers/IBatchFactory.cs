namespace TMenos3.NetCore.ApiDemo.Services.Helpers
{
    internal interface IBatchFactory
    {
        Batch<TInput, TOutput> Create<TInput, TOutput>(int size);
    }
}
