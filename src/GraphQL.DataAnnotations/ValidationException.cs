using System;

class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}