using System;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.Logging
{
    [AttributeUsage(
        AttributeTargets.Parameter | 
        AttributeTargets.ReturnValue | 
        AttributeTargets.Property, 
        AllowMultiple = false, 
        Inherited = false)]
    public sealed class SkipLogAttribute : Attribute
    {
    }
}
