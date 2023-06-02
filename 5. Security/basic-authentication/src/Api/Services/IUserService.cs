namespace Api.Services;

public interface IUserService
{
    (bool, string) AuthenticateUser(string username, string password);
}
