﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands.CreateAdminMember;

public class CreateAdminMemberCommandHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_AddsNewAdmin(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        CreateAdminMemberCommandHandler sut,
        CreateAdminMemberCommand command)
    {
        var response = await sut.Handle(command, new CancellationToken());
        response.Result.MemberId.Should().Be(command.MemberId);

        membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.MemberId)));
        membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.UserType == UserType.Admin)));
        auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.MemberId)));
    }
}