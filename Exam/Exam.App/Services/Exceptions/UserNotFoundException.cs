namespace Exam.App.Services.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string username) : base($"Item with username {username} could not be found.")
    {
    }
}