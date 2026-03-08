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
    Person Person,
    string Username,
    string RefreshTokenHash,
    DateTime RefreshTokenExpiredAt,
    DateTime? RefreshTokenRevokedAt,
    string Role,
    string Password,
    bool IsActive);
}
