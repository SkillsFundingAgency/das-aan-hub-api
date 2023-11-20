using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.MemberProfilesControllerTests;
public class MemberProfilesControllerPutTests
{
    [Test, MoqAutoData]
    public async Task Put_InvalidMemberId_ReturnsAllErrors(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] MemberProfilesController sut,
    [Frozen] List<ValidationFailure> errors,
    CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(errors);

        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateMemberProfilesCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PutMemberProfile(Guid.NewGuid(), new UpdateMemberProfileModel(), cancellationToken) as BadRequestObjectResult;

        result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        result.Value.Should().BeEquivalentTo(errors.Select(e => new { e.ErrorMessage, e.PropertyName }));
    }

    [Test, MoqAutoData]
    public async Task Put_UpdatesMemberProfile_Returns204NoContent(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MemberProfilesController sut,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(true));

        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateMemberProfilesCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PutMemberProfile(Guid.NewGuid(), new UpdateMemberProfileModel(), cancellationToken) as NoContentResult;

        result?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
