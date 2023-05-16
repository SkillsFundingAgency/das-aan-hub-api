using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Models;

public record AttendeeModel(Guid MemberId, string UserType, string MemberName)
{
    public static implicit operator AttendeeModel(Attendance source) => new(source.MemberId, source.Member.UserType, source.Member.FullName);
}

