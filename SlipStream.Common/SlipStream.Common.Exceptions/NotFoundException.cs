namespace SlipStream.Common;

public class NotFoundException(string message) : AppException(message);