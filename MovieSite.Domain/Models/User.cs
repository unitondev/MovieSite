﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace MovieSite.Domain.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(25)]
        public string FullName { get; set; }

        public byte[] Avatar { get; set; } = Encoding.UTF8.GetBytes(BaseAvatar);
        [JsonIgnore]
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public IList<UserRating> UserRatings { get; set; }
        public ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        const string BaseAvatar = "data:image/png;base64,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";
    }
}