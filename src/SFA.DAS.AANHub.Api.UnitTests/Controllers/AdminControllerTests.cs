using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Admins.Commands;
using SFA.DAS.AANHub.Application.UnitTests;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class AdminControllerTests
    {

        [Test, AutoMoqData]
        public async Task CreateAdmin_InvokesRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] AdminsController sut,
            CreateAdminModel model, CreateAdminMemberCommand command, string userName)
        {
            var response = new CreateAdminMemberCommandResponse()
            {
                MemberId = command.Id,
                Status = MembershipStatus.Live.ToString(),
            };

            mediatorMock.Setup(m => m.Send(It.Is<CreateAdminMemberCommand>(c => c.UserName == userName), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await sut.CreateAdmin(Guid.NewGuid(), model);

            result.As<CreatedAtActionResult>().ControllerName.Should().Be("Admins");
            result.As<CreatedAtActionResult>().ActionName.Should().Be("CreateAdmin");
            result.As<CreatedAtActionResult>().StatusCode.Should().Be(201);
        }

    }
}
