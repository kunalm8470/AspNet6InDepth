namespace Api.Services;

public class UserService : IUserService
{
    private readonly Dictionary<string, (string, string)> _users = new()
    {
        ["admin"] = ("admin", "Admin"),
        ["johndoe"] = ("dummypassword@1234", "User"),
        ["janedoe"] = ("dummypassword@5678", "User"),
    };

    public (bool, string) AuthenticateUser(string username, string password)
    {
        // Check for user
        if (!_users.TryGetValue(username, out (string, string) credentials))
        {
            return (false, null);
        }

        // Check for password
        if (password != credentials.Item1)
        {
            return (false, null);
        }

        return (true, credentials.Item2);
    }
}
