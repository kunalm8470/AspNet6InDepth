namespace Web.Services;

public interface IUserService
{
    (bool, string) AuthenticateUser(string email, string password);
}
