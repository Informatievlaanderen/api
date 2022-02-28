namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json;

    public class JsonErrorActionFilter : IActionFilter, IOrderedFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Result == null && !context.ModelState.IsValid
                && HasJsonErrors(context.ModelState))
            {
                throw new ValidationException(new List<ValidationFailure>(new []
                {
                    new ValidationFailure("", "Json is not valid.") { ErrorCode = "JsonInvalid" }
                }));
            }
        }

        private static bool HasJsonErrors(ModelStateDictionary modelState)
        {
            foreach (var entry in modelState.Values)
            {
                if (entry.Errors.Any(error => error.Exception is JsonException))
                {
                    return true;
                }
            }

            return false;
        }

        // Set to a large negative value so it runs earlier than ModelStateInvalidFilter
        public int Order => -200000;
    }}
