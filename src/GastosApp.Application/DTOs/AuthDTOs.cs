namespace GastosApp.Application.DTOs;

public record RegisterRequest(string Nombre, string Email, string Password);

public record LoginRequest(string Email, string Password);

public record AuthResponse(string Token, DateTime ExpiraEn, string Nombre, string Email);
