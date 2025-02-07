﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Kvikmynd.Domain.Models
{
    [Owned]
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(128)]
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => Revoked == null && !IsExpired;
    }
}