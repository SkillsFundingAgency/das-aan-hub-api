﻿using SFA.DAS.AANHub.Application.Apprentices.Commands;

namespace SFA.DAS.AANHub.Api.Models
{
    public class CreateApprenticeModel
    {
        public long ApprenticeId { get; set; }
        public DateTime Joined { get; set; }
        public int? Region { get; set; }
        public string? Information { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;

        public static implicit operator CreateApprenticeMemberCommand(CreateApprenticeModel model) => new CreateApprenticeMemberCommand()
        {
            ApprenticeId = model.ApprenticeId,
            Joined = model.Joined,
            Information = model.Information,
            Email = model.Email,
            Name = model.Name
        };
    }
}