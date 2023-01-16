﻿using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands
{
    public class CreateApprenticeMemberCommandValidator : AbstractValidator<CreateApprenticeMemberCommand>
    {
        public const string ApprenticeAlreadyExistsErrorMessage = "ApprenticeId already exists";

        public CreateApprenticeMemberCommandValidator(IMembersReadRepository membersReadRepository, IRegionsReadRepository regionsReadRepository, IApprenticesReadRepository apprenticesReadRepository)
        {
            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));
            Include(new RequestedByMemberIdValidator(membersReadRepository));

            RuleFor(c => c.ApprenticeId)
            .NotEmpty()
            .MustAsync(async (ApprenticeId, cancellation) =>
            {
                var apprentice = await apprenticesReadRepository.GetApprentice(ApprenticeId);
                return apprentice == null;
            })
            .WithMessage(ApprenticeAlreadyExistsErrorMessage);
        }
    }
}
