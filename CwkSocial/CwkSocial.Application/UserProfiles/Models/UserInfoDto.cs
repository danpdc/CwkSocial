using Cwk.Domain.Aggregates.UserProfileAggregate;

namespace CwkSocial.Application.UserProfiles.Models;

public class UserInfoDto
{
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? EmailAddress { get; private set; }
    public string? Phone { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? CurrentCity { get; private set; }

    public static UserInfoDto FromBasicInfo(BasicInfo basicInfo)
    {
        return new UserInfoDto
        {
            FirstName = basicInfo.FirstName,
            LastName = basicInfo.LastName,
            EmailAddress = basicInfo.EmailAddress,
            Phone = basicInfo.Phone,
            DateOfBirth = basicInfo.DateOfBirth,
            CurrentCity = basicInfo.CurrentCity
        };
    }
}