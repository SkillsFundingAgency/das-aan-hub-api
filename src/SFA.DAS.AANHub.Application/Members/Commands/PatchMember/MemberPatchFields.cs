using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Application.Members.Commands.PatchMember;

[ExcludeFromCodeCoverage]
public static class MemberPatchFields
{
    public static readonly string FirstName = nameof(FirstName).ToLower();
    public static readonly string Email = nameof(Email).ToLower();
    public static readonly string LastName = nameof(LastName).ToLower();
    public static readonly string OrganisationName = nameof(OrganisationName).ToLower();
    public static readonly string RegionId = nameof(RegionId).ToLower();
    public static readonly string Status = nameof(Status).ToLower();
    public static readonly string ReceiveNotifications = nameof(ReceiveNotifications).ToLower();
    public static readonly string[] PatchFields = new[]
    {
        Email,
        FirstName,
        LastName,
        OrganisationName,
        RegionId,
        Status,
        ReceiveNotifications
    };
}
