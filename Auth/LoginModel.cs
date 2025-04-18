using System.ComponentModel.DataAnnotations;

namespace Fitness.Auth;

public class LoginModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}