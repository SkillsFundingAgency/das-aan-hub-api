using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.MemberProfilesControllerTests;
public class MemberProfilesControllerGetTests
{
    [Test, MoqAutoData]
    public async Task GetMemberProfileWithPreferences_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MemberProfilesController sut,
        Guid memberId,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        await sut.GetMemberProfileWithPreferences(memberId, requestedByMemberId, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetMemberProfilesWithPreferencesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()));
    }

    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task GetMemberProfileWithPreferences_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        bool IsPublicView,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MemberProfilesController sut,
        Guid memberId,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        var notFoundResponse = ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberProfilesWithPreferencesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>())).ReturnsAsync(notFoundResponse);

        var result = await sut.GetMemberProfileWithPreferences(memberId, requestedByMemberId, cancellationToken, IsPublicView);

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task GetMemberProfileWithPreferences_HandlerReturnsData_ReturnsOkResponse(
        bool IsPublicView,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MemberProfilesController sut,
        Guid memberId,
        Guid requestedByMemberId,
        GetMemberProfilesWithPreferencesQueryResult getMemberProfilesWithPreferencesQueryResult,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult>(getMemberProfilesWithPreferencesQueryResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberProfilesWithPreferencesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetMemberProfileWithPreferences(memberId, requestedByMemberId, cancellationToken, IsPublicView);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getMemberProfilesWithPreferencesQueryResult);
    }

    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task GetMemberProfileWithPreferences_InvalidRequest_ReturnsBadRequestResponse(
        bool IsPublicView,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MemberProfilesController sut,
        List<ValidationFailure> errors,
        Guid memberId,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberProfilesWithPreferencesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetMemberProfileWithPreferences(memberId, requestedByMemberId, cancellationToken, IsPublicView);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
