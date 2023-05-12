﻿using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Models;

public record AttendeeModel(Guid AttendanceId, Guid MemberId, string MemberName)
{
    public static implicit operator AttendeeModel(Attendance source) => new(source.Id, source.MemberId, source.Member.FullName);
}
