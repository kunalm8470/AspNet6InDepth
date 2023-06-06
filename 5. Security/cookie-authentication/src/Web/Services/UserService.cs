namespace Web.Services;

public class UserService : IUserService
{
    private readonly Dictionary<string, (string, string)> _users = new()
    {
        ["admin@fakesystem.com"] = ("admin", "Admin"),
        ["johndoe@fakesystem.com"] = ("dummypassword@1234", "User"),
        ["janedoe@fakesystem.com"] = ("dummypassword@5678", "User"),
    };

    public (bool, string) AuthenticateUser(string email, string password)
    {
        // Check for email
        if (!_users.TryGetValue(email, out (string, string) credentials))
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
