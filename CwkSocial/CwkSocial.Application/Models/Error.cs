using CwkSocial.Application.Enums;

namespace CwkSocial.Application.Models;

public class Error
{
    public ErrorCode Code { get; set; }
    public string Message { get; set; }
}