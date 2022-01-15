using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.Identity.Commands;

public class LoginCommand : IRequest<OperationResult<string>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}