using FluentValidation;

namespace DemoWebApi.Providers
{
    public interface IValidationProvider
    {
        IValidator[] GetValidators();
    }

    public class ValidationProvider : IValidationProvider
    {
        private readonly IValidator[] _validators;

        public ValidationProvider(IValidator[] validators)
        {
            _validators = validators;
        }

        public IValidator[] GetValidators()
        {
            return _validators;
        }
    }
}
