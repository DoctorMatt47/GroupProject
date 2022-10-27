using System.Security.Claims;

namespace GroupProject.Application.Common.Interfaces;

public interface ITokenRepository
{
    string Get(ClaimsIdentity identity);
}
