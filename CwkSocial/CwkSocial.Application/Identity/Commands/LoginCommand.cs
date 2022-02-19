using CwkSocial.Application.Models;
using CwkSocial.Application.Models.Identity;
using MediatR;

namespace CwkSocial.Application.Identity.Commands;

public class LoginCommand : IRequest<OperationResult<LoginResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}