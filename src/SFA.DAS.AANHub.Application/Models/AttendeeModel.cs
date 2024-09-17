using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Models;

public record AttendeeModel(Guid MemberId, UserType UserType, string MemberName, DateTime? AddedDate, DateTime? CancelledDate)
{
    public static implicit operator AttendeeModel(Attendance source) => new(source.MemberId, source.Member.UserType, source.Member.FullName, source.AddedDate, source.CancelledDate);
}
