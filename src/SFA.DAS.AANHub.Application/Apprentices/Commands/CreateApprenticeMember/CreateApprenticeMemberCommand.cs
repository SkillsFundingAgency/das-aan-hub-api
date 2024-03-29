﻿using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;

public class CreateApprenticeMemberCommand : CreateMemberCommandBase, IRequest<ValidatedResponse<CreateMemberCommandResponse>>
{
    public Guid ApprenticeId { get; set; }

    public static implicit operator Member(CreateApprenticeMemberCommand command) => new()
    {
        Id = command.MemberId,
        UserType = UserType.Apprentice,
        Status = MembershipStatus.Live,
        Email = command.Email!,
        FirstName = command.FirstName!,
        LastName = command.LastName!,
        JoinedDate = command.JoinedDate!.Value,
        RegionId = command.RegionId,
        OrganisationName = command.OrganisationName,
        IsRegionalChair = false,
        Apprentice = new Apprentice
        {
            MemberId = command.MemberId,
            ApprenticeId = command.ApprenticeId
        },
        MemberProfiles = command.ProfileValues.Select(p => ProfileConverter(p, command.MemberId)).ToList()
    };

    private static MemberProfile ProfileConverter(ProfileValue source, Guid memberId) => new() { MemberId = memberId, ProfileId = source.Id, ProfileValue = source.Value };
}