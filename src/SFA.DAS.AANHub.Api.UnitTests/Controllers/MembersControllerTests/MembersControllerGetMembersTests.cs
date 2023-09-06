using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.MembersControllerTests;
public class MembersControllerGetMembersTests
{
    [Test, MoqAutoData]
    public async Task Get_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] MembersController sut,
       Guid requestedByMemberId,
       List<MemberUserType> userType,
       List<MembershipStatusType> status,
       bool? isRegionalChair,
       List<int> regionIds,
       string keyword,
       CancellationToken cancellationToken)
    {
        var getMembersModel = new GetMembersModel
        {
            RequestedByMemberId = requestedByMemberId,
            UserType = userType,
            Status = status,
            IsRegionalChair = isRegionalChair,
            RegionId = regionIds,
            Keyword = keyword
        };

        await sut.GetMembers(getMembersModel, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetMembersQuery>(q => q.RequestedByMemberId == requestedByMemberId),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsNullResult_ReturnsEmptyResultResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid requestedByMemberId,
        List<MemberUserType> userType,
        List<MembershipStatusType> status,
        bool? isRegionalChair,
        List<int> regionIds,
        string keyword,
        CancellationToken cancellationToken)
    {
        var emptyResponse = ValidatedResponse<GetMembersQueryResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.Is<GetMembersQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(emptyResponse);

        var getMembersModel = new GetMembersModel
        {
            RequestedByMemberId = requestedByMemberId,
            UserType = userType,
            Status = status,
            IsRegionalChair = isRegionalChair,
            RegionId = regionIds,
            Keyword = keyword
        };

        var result = await sut.GetMembers(getMembersModel, cancellationToken);
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid requestedByMemberId,
        GetMembersQueryResult queryResult,
        List<MemberUserType> userType,
        List<MembershipStatusType> status,
        bool? isRegionalChair,
        List<int> regionIds,
        string keyword,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<GetMembersQueryResult>(queryResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMembersQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        var getMembersModel = new GetMembersModel
        {
            RequestedByMemberId = requestedByMemberId,
            UserType = userType,
            Status = status,
            IsRegionalChair = isRegionalChair,
            RegionId = regionIds,
            Keyword = keyword
        };

        var result = await sut.GetMembers(getMembersModel, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }

    [Test, MoqAutoData]
    public async Task Get_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        List<ValidationFailure> errors,
        Guid requestedByMemberId,
        GetMembersQueryResult queryResult,
        List<MemberUserType> userType,
        List<MembershipStatusType> status,
        bool? isRegionalChair,
        List<int> regionIds,
        string keyword,
        CancellationToken cancellationToken)
    {
        var errorResponse = new ValidatedResponse<GetMembersQueryResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetMembersQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(errorResponse);

        var getMembersModel = new GetMembersModel
        {
            RequestedByMemberId = requestedByMemberId,
            UserType = userType,
            Status = status,
            IsRegionalChair = isRegionalChair,
            RegionId = regionIds,
            Keyword = keyword
        };

        var result = await sut.GetMembers(getMembersModel, cancellationToken);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }


    [TestCase(1, 2, 1, 2)]
    [TestCase(0, 2, 1, 2)]
    [TestCase(-1, 2, 1, 2)]
    [TestCase(0, 2, 1, 2)]
    [TestCase(1, 6, 1, 6)]
    [TestCase(2, 6, 2, 6)]
    [TestCase(1, 0, 1, 30)]
    [TestCase(1, -1, 1, 30)]
    public async Task Get_SetsPageAndPageSizeAsExpected(int page, int pageSize, int expectedPage, int expectedPageSize)
    {
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<MembersController>>();
        var sut = new MembersController(loggerMock.Object, mediatorMock.Object);
        var result = new ValidatedResponse<GetMembersQueryResult>();

        mediatorMock.Setup(x => x.Send(It.IsAny<GetMembersQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(result);

        var getMembersModel = new GetMembersModel
        {
            RequestedByMemberId = Guid.NewGuid(),
            UserType = new List<MemberUserType>(),
            Status = new List<MembershipStatusType>(),
            IsRegionalChair = null,
            RegionId = new List<int>(),
            Keyword = string.Empty,
            Page = page,
            PageSize = pageSize
        };

        await sut.GetMembers(getMembersModel, new CancellationToken());

        mediatorMock.Verify(
            m => m.Send(It.Is<GetMembersQuery>(q => q.Page == expectedPage && q.PageSize == expectedPageSize),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Get_DefaultsSetsPageAndPageSizeAsExpected()
    {
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<MembersController>>();
        var sut = new MembersController(loggerMock.Object, mediatorMock.Object);
        var result = new ValidatedResponse<GetMembersQueryResult>();
        var defaultPage = 1;
        mediatorMock.Setup(x => x.Send(It.IsAny<GetMembersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var getMembersModel = new GetMembersModel
        {
            RequestedByMemberId = Guid.NewGuid(),
            UserType = new List<MemberUserType>(),
            Status = new List<MembershipStatusType>(),
            IsRegionalChair = null,
            RegionId = new List<int>(),
            Keyword = string.Empty,
        };

        await sut.GetMembers(getMembersModel, new CancellationToken());

        mediatorMock.Verify(
            m => m.Send(It.Is<GetMembersQuery>(q => q.Page == defaultPage && q.PageSize == Constants.Members.PageSize),
                It.IsAny<CancellationToken>()));
    }
}