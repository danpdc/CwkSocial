using CwkSocial.Application.Models;
using CwkSocial.Application.Models.Identity;
using MediatR;

namespace CwkSocial.Application.Identity.Commands;

public class RegisterIdentity : IRequest<OperationResult<RegisterResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string CurrentCity { get; set; }
}