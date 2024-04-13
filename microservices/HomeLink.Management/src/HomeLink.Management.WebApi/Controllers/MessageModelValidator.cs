using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NetFusion.Common.Extensions.Reflection;
using NetFusion.Messaging.Types.Contracts;

namespace HomeLink.Management.WebApi.Controllers;

public class MessageModelValidator : IModelValidatorProvider
{
    public void CreateValidators(ModelValidatorProviderContext context)
    {
        if (context.ModelMetadata.ContainerType?.IsDerivedFrom(typeof(IMessage)) ?? false)
        {
            context.Results.Clear();
        }
    }
}

