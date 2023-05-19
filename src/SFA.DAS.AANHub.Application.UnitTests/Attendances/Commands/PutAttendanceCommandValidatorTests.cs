using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Commands
{
    public class PutAttendanceCommandValidatorTests
    {
        [Test]
        [RecursiveMoqAutoData]
        public async Task Validate_MissingOrInvalidMemberId_Fails(
             Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
             Mock<IMembersReadRepository> membersReadRepository)
        {
            calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(It.IsAny<Guid>()))
                                        .ReturnsAsync(new CalendarEvent() { IsActive = true, StartDate = DateTime.Now.AddMonths(1) });

            var command = new PutAttendanceCommand(Guid.NewGuid(), Guid.Empty, true);

            var sut = new PutAttendanceCommandValidator(calendarEventsReadRepository.Object, membersReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId);
        }

        [TestCase(MembershipStatus.Deleted)]
        [TestCase(MembershipStatus.Cancelled)]
        [TestCase(MembershipStatus.Pending)]
        [TestCase(MembershipStatus.Withdrawn)]
        public async Task Validate_InactiveMemberId_Fails(string inactiveStatus)
        {
            Member inactiveMember = new() { Status = inactiveStatus, Id = Guid.NewGuid() };

            Mock<ICalendarEventsReadRepository> calendarEventsReadRepository = new();
            calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(It.IsAny<Guid>()))
                                        .ReturnsAsync(new CalendarEvent() { IsActive = true, StartDate = DateTime.Now.AddMonths(1) });

            Mock<IMembersReadRepository> membersReadRepository = new();
            membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>()))
                                 .ReturnsAsync(inactiveMember);

            var command = new PutAttendanceCommand(Guid.NewGuid(), Guid.Empty, true);

            var sut = new PutAttendanceCommandValidator(calendarEventsReadRepository.Object, membersReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId);
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task Validate_MissingEventId_Fails(
             Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
             Mock<IMembersReadRepository> membersReadRepository)
        {
            membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>()))
                                 .ReturnsAsync(new Member() { Status = MembershipStatus.Live });

            var command = new PutAttendanceCommand(Guid.Empty, Guid.NewGuid(), true);

            var sut = new PutAttendanceCommandValidator(calendarEventsReadRepository.Object, membersReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.CalendarEventId);
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task Validate_EventInactive_Fails(
             Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
             Mock<IMembersReadRepository> membersReadRepository)
        {
            calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(It.IsAny<Guid>()))
                                        .ReturnsAsync(new CalendarEvent() { IsActive = false, StartDate = DateTime.Now.AddMonths(1) });

            membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>()))
                                 .ReturnsAsync(new Member() { Status = MembershipStatus.Live });

            var command = new PutAttendanceCommand(Guid.Empty, Guid.NewGuid(), true);

            var sut = new PutAttendanceCommandValidator(calendarEventsReadRepository.Object, membersReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.CalendarEventId);
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task Validate_EventNotFound_Fails(
             Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
             Mock<IMembersReadRepository> membersReadRepository)
        {
            calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(It.IsAny<Guid>()))
                                        .ReturnsAsync(() => null);

            membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>()))
                                 .ReturnsAsync(new Member() { Status = MembershipStatus.Live });

            var command = new PutAttendanceCommand(Guid.Empty, Guid.NewGuid(), true);

            var sut = new PutAttendanceCommandValidator(calendarEventsReadRepository.Object, membersReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.CalendarEventId);
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task Validate_EventInThePast_Fails(
             Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
             Mock<IMembersReadRepository> membersReadRepository)
        {
            calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(It.IsAny<Guid>()))
                                        .ReturnsAsync(new CalendarEvent() { IsActive = true, StartDate = new DateTime(1942, 01, 01) });

            membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>()))
                                 .ReturnsAsync(new Member() { Status = MembershipStatus.Live });

            var command = new PutAttendanceCommand(Guid.Empty, Guid.NewGuid(), true);

            var sut = new PutAttendanceCommandValidator(calendarEventsReadRepository.Object, membersReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.CalendarEventId);
        }
    }
}
