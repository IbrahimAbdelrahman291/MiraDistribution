

namespace MiraDistribution.Application.Common.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message = "رقم التليفون أو كلمة المرور غلط.")
            : base(message) { }
    }
}
