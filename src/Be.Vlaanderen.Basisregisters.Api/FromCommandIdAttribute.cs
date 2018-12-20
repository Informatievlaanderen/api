namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Threading.Tasks;
    using AspNetCore.Mvc.ModelBinding.GuidHeader;
    using Generators.Guid;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class FromCommandIdAttribute : Attribute, IModelNameProvider, IBinderTypeProviderMetadata
    {
        public BindingSource BindingSource => BindingSource.Header;

        public string Name => CustomHeaderNames.CommandId;

        public Type BinderType => typeof(CommandIdHeaderModelBinder);
    }

    public class CommandIdHeaderModelBinder : GuidHeaderModelBinder, IModelBinder
    {
        public new Task BindModelAsync(ModelBindingContext bindingContext)
        {
            base.BindModelAsync(bindingContext).GetAwaiter().GetResult();

            if (bindingContext.Result.IsModelSet)
                return Task.CompletedTask;

            bindingContext.Result = ModelBindingResult.Success(
                Deterministic.Create(
                    Deterministic.Namespaces.Commands,
                    bindingContext.HttpContext.TraceIdentifier));

            return Task.CompletedTask;
        }
    }
}
