using EnsureThat;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    internal class TrimmingModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Ensure.Any.IsNotNull(bindingContext, nameof(bindingContext));
            Ensure.Type.IsString(bindingContext.ModelType, nameof(bindingContext.ModelType));

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            bindingContext.Result = ModelBindingResult.Success(
                string.IsNullOrWhiteSpace(valueProviderResult.FirstValue) ?
                null :
                valueProviderResult.FirstValue.Trim());

            return Task.CompletedTask;
        }
    }
}
