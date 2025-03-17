using System;

namespace JobPortal.CQRS.Auth.DTOs
{
    /// <summary>
    /// Data Transfer Object for authentication response containing JWT token
    /// </summary>
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
} 