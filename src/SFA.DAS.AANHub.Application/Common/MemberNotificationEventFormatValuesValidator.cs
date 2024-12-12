using FluentValidation;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.Common
{
    public class MemberNotificationEventFormatValuesValidator : AbstractValidator<MemberNotificationEventFormatValues>
    {
        public const string InvalidMemberNotificationEventFormatErrorMessage =
            "Member notification event format must be one of the following: InPerson, Online, Hybrid, All.";

        public MemberNotificationEventFormatValuesValidator()
        {
            RuleFor(p => p.EventFormat)
                .Must(BeAValidEventFormat)
                .WithMessage(InvalidMemberNotificationEventFormatErrorMessage);
        }

        private bool BeAValidEventFormat(string eventFormat)
        {
            if (eventFormat.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return Enum.TryParse(typeof(EventFormat), eventFormat, true, out _);
        }
    }
}