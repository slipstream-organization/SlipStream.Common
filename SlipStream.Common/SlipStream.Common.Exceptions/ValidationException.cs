namespace SlipStream.Common;

public class ValidationException : AppException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; set; }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors): base("One or more validation errors")
    {
        Errors = errors;
    }
}