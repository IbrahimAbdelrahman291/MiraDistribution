using FluentValidation.Results;

namespace MiraDistribution.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException() : base("حدث خطأ أو أكتر في التحقق من البيانات.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }
    }
}
