using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IJWTManagerRepository
{
    Tokens Authenticate(Users users);
}
