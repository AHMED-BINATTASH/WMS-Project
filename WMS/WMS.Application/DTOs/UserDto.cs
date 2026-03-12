using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Application.DTOs
{
    public record UserDto(int UserID,
    int PersonID,
    string Username,
    string? RefreshTokenHash,
    DateTime? RefreshTokenExpiredAt,
    DateTime? RefreshTokenRevokedAt,
    string Role,
    string Password,
    bool IsActive);

    public record UserAddDto(int UserID,
    int PersonID,
    string Username,
    string Role,
    string Password,
    bool IsActive);

    public record UserSlimDto(int UserID,
    int PersonID,
    string Username,
    string Role,
    bool IsActive);
}
