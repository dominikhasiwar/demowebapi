using DemoWebApi.Providers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoWebApi.Attributes
{
    public class OperationValidatorAttribute: ActionFilterAttribute
    {
        private readonly IValidationProvider _validationProvider;

        public OperationValidatorAttribute(IValidationProvider validationProvider)
        {
            _validationProvider = validationProvider;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var payload = GetPayload(context);

            if (payload == null)
            {
                return;
            }

            var validators = _validationProvider.GetValidators();

            if (validators.Length == 0)
            {
                return;
            }

            var validationContext = new ValidationContext<object>(payload);

            var errors = validators
                        ?.Where(x => x.CanValidateInstancesOfType(payload.GetType()))
                        ?.Select(x => x.Validate(validationContext))
                        ?.SelectMany(result => result.Errors)
                        ?.ToList();

            if (errors?.Count > 0)
            {
                throw new ValidationException("One or more validation errors occured!", errors);
            }           
        }

        private object GetPayload(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue("model", out var payload))
            {
                context.ActionArguments.TryGetValue("query", out payload);
            }

            return payload;
        }
    }
}
