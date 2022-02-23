using System.Text.Json.Serialization;

namespace practice.domain.Entities;

public class RefreshToken
{
    public int Id { get; set;}
    public string  UserId { get; set; } // user id when logged in
    public string Token { get; set; }
    public string JwtId { get; set; } // the id generated when a jwt id has been requested
    public bool IsActive { get; set; } // to make sure that the token is only used once
    public bool IsRevoked { get; set; } // make sure the are valid
    public DateTime Created { get; set; }
    public DateTime Expires { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsExpired { get; set; }

    
}