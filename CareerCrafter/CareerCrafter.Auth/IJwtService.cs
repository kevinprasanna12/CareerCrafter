namespace CareerCrafter.Auth;

public interface IJwtService
{
    string GenerateToken(int userId,string username, string role);
}
