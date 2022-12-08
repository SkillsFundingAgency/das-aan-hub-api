using FluentValidation;
using SFA.DAS.AANHub.Application.Common;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands
{
    internal class CreateApprenticeCommandValidator : AbstractValidator<CreateApprenticesCommand>
    {
        public CreateApprenticeCommandValidator()
        {
            Include(new CreateMemberCommandValidator());

            RuleFor(c => c.Email).NotEmpty();
        }
    }
}
