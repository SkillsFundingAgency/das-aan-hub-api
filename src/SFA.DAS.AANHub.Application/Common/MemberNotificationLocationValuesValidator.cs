using FluentValidation;
using System.Text.RegularExpressions;

namespace SFA.DAS.AANHub.Application.Common
{
    public class MemberNotificationLocationValuesValidator : AbstractValidator<MemberNotificationLocationValues>
    {
        public const string InvalidMemberNotificationEventFormatErrorMessage = "Member notification location name contains invalid characters. Only letters, numbers, and spaces are allowed.";

        public MemberNotificationLocationValuesValidator()
        {
            RuleFor(p => p.Name)
                .Must(BeAValidName)
                .WithMessage(InvalidMemberNotificationEventFormatErrorMessage);
        }

        private bool BeAValidName(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z0-9\s,']+$");
        }
    }
}