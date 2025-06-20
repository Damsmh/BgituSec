﻿using BgituSec.Application.DTOs;
using System.Security.Claims;

namespace BgituSec.Api.Services
{
    public interface ITokenService
    {
        string CreateToken(UserDTO user);
        RefreshTokenDTO GenerateRefreshToken(int userId);
        string Hash(string refreshToken);
        bool Verify(string stored, string old);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
