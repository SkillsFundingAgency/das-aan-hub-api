using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Common;

[ExcludeFromCodeCoverage]
public static class Constants
{
    public static class MembershipStatus
    {
        public const string Pending = nameof(Pending);
        public const string Cancelled = nameof(Cancelled);
        public const string Live = nameof(Live);
        public const string Withdrawn = nameof(Withdrawn);
        public const string Deleted = nameof(Deleted);
    }

    public static class CalendarEvents
    {
        public static readonly int PageSize = 5;
    }

    public static class ProfileDataId
    {
        public const int Longitude = 36;
        public const int Latitude = 37;
    }

    public static class Members
    {
        public static readonly int PageSize = 30;
    }

    public static class EmailTemplateName
    {
        public const string ApprenticeOnboardingTemplate = "AANApprenticeOnboarding";
        public const string EmployerOnboardingTemplate = "AANEmployerOnboarding";
        public const string EmployerEventSignUpTemplate = "AANEmployerEventSignup";
        public const string EmployerEventCancelTemplate = "AANEmployerEventCancel";
        public const string ApprenticeEventSignUpTemplate = "AANApprenticeEventSignup";
        public const string ApprenticeEventCancelTemplate = "AANApprenticeEventCancel";
        public const string ApprenticeWithdrawal = "AANApprenticeWithdrawal";
        public const string EmployerWithdrawal = "AANEmployerWithdrawal";
    }
}
