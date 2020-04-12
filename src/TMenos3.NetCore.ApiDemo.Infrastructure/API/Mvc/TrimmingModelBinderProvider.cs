using EnsureThat;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    internal class TrimmingModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            Ensure.Any.IsNotNull(context, nameof(context));

            if (!context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(string))
            {
                return new TrimmingModelBinder();
            }

            return null;
        }
    }
}
