namespace Models.Repositories
{
    public interface IJWTAuthenticationService
    {
        string Authenticate(string username, string password);
    }
}