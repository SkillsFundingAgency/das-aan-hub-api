using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Queries.GetMemberAttendances;

public class GetMemberAttendancesQueryValidatorTests
{
    [TestCase("0001-01-01", true)]
    [TestCase(null, false)]
    public async Task Validate_FromDate(DateTime? date, bool isValid)
    {
        GetMemberAttendancesQuery target = new(Guid.NewGuid(), date, DateTime.Today);

        GetMemberAttendancesQueryValidator sut = new(Mock.Of<IMembersReadRepository>());

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(r => r.FromDate);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(r => r.FromDate).WithErrorMessage(GetMemberAttendancesQueryValidator.FromDateIsRequired);
        }
    }

    [TestCase("1001-01-01", true)]
    [TestCase(null, false)]
    public async Task Validate_ToDate(DateTime? date, bool isValid)
    {
        GetMemberAttendancesQuery target = new(Guid.NewGuid(), DateTime.MinValue, date);

        GetMemberAttendancesQueryValidator sut = new(Mock.Of<IMembersReadRepository>());

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(r => r.ToDate);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(r => r.ToDate).WithErrorMessage(GetMemberAttendancesQueryValidator.ToDateIsRequired);
        }
    }

    [TestCase("0001-01-01", "0001-01-02", true)]
    [TestCase("0001-01-02", "0001-01-01", false)]
    public async Task Validate_DateRange(DateTime fromDate, DateTime toDate, bool isValid)
    {
        GetMemberAttendancesQuery target = new(Guid.NewGuid(), fromDate, toDate);

        GetMemberAttendancesQueryValidator sut = new(Mock.Of<IMembersReadRepository>());

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(r => r.ToDate);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(r => r.ToDate).WithErrorMessage(GetMemberAttendancesQueryValidator.IncorrectDateRange);
        }
    }
}
