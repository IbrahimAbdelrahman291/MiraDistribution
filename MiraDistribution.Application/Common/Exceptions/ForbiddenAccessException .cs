

namespace MiraDistribution.Application.Common.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base("مفيش صلاحية للقيام بهذا الإجراء.") { }
    }
}
