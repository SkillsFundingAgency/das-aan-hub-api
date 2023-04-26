using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Extensions;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Members.Commands.PatchMember;

public class PatchMemberCommand : IRequest<ValidatedResponse<PatchCommandResult>>
{
    public Guid MemberId { get; set; }
    public JsonPatchDocument<Member> PatchDoc { get; set; } = new();

    public string? Email => PatchDoc.GetReplacementValue(MemberPatchFields.Email);
    public string? FirstName => PatchDoc.GetReplacementValue(MemberPatchFields.FirstName);
    public string? LastName => PatchDoc.GetReplacementValue(MemberPatchFields.LastName);
    public string? OrganisationName => PatchDoc.GetReplacementValue(MemberPatchFields.OrganisationName);
    public int? RegionId => PatchDoc.GetReplacementValueAsInt(MemberPatchFields.RegionId);
    public string? Status => PatchDoc.GetReplacementValue(MemberPatchFields.Status);


    public bool HasEmail => PatchDoc.HasValue(MemberPatchFields.Email);
    public bool HasFirstName => PatchDoc.HasValue(MemberPatchFields.FirstName);
    public bool HasLastName => PatchDoc.HasValue(MemberPatchFields.LastName);
    public bool HasOrganisationName => PatchDoc.HasValue(MemberPatchFields.OrganisationName);
    public bool HasRegionId => PatchDoc.HasValue(MemberPatchFields.RegionId);
    public bool HasStatus => PatchDoc.HasValue(MemberPatchFields.Status);
}

