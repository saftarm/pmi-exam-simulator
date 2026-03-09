using TestAPI.Models;
using TestAPI.Entities;
namespace TestAPI.Services.Interfaces
{
    public interface IJWTService
    {
        string GenerateToken(User user);
    }
}
