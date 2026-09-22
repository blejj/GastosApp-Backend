using GastosApp.Domain.Entities;

namespace GastosApp.Application.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiraEn) GenerarToken(Usuario usuario);
}
