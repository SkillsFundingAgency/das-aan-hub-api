using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.StagedApprentices.Queries;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.UnitTests;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class StagedApprenticesControllerTests
    {
        [Test]
        [AutoMoqData]
        public async Task GetApprentice_InvokesQueryHandler(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] StagedApprenticesController sut,
            string lastname, DateTime dateofbirth, string email,
            ValidatedResponse<GetStagedApprenticeQueryResult> handlerResult)
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<GetStagedApprenticeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(handlerResult);

            var response = await sut.GetStagedApprentice(lastname, dateofbirth, email);
            response.Should().NotBeNull();

            var result = response as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);

            var queryResult = result.Value as GetStagedApprenticeQueryResult;
            queryResult.Should().BeEquivalentTo(handlerResult.Result);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetStagedApprenticeQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetApprentice_InvokesQueryHandler_NoResultGivesNotFound(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] StagedApprenticesController sut,
            string lastname, DateTime dateofbirth, string email)
        {
            var errorResponse = new ValidatedResponse<GetStagedApprenticeQueryResult>
                (new List<ValidationFailure>());

            mediatorMock.Setup(m => m.Send(It.Is<GetStagedApprenticeQuery>(q => q.LastName == lastname
                                                                                    && q.DateOfBirth == dateofbirth
                                                                                    && q.Email == email), It.IsAny<CancellationToken>()))
                                                                                    .ReturnsAsync(errorResponse);

            var response = await sut.GetStagedApprentice(lastname, dateofbirth, email);

            var result = response as NotFoundResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status404NotFound, result!.StatusCode);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetStagedApprenticeQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetApprentice_InvokesQueryHandler_ResultGivesSuccessfulResult(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] StagedApprenticesController sut,
            string lastname, DateTime dateofbirth, string email,
            GetStagedApprenticeQueryResult getStagedApprenticeResult)
        {
            var response = new ValidatedResponse<GetStagedApprenticeQueryResult>(getStagedApprenticeResult);

            mediatorMock.Setup(m => m.Send(It.Is<GetStagedApprenticeQuery>(q => q.LastName == lastname
                                                                                    && q.DateOfBirth == dateofbirth
                                                                                    && q.Email == email), It.IsAny<CancellationToken>()))
                                                                                    .ReturnsAsync(response);

            var getResult = await sut.GetStagedApprentice(lastname, dateofbirth, email);

            var result = getResult as OkObjectResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        }

        [Test]
        [AutoMoqData]
        public async Task GetApprentice_InvokesQueryHandler_BadResultGivesBadRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] StagedApprenticesController sut,
            string lastname, DateTime dateofbirth, string email)
        {
            var errorResponse = new ValidatedResponse<GetStagedApprenticeQueryResult>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });

            mediatorMock.Setup(m => m.Send(It.Is<GetStagedApprenticeQuery>(q => q.LastName == lastname
                                                                                    && q.DateOfBirth == dateofbirth
                                                                                    && q.Email == email), It.IsAny<CancellationToken>()))
                                                                                    .ReturnsAsync(errorResponse);

            var response = await sut.GetStagedApprentice(lastname, dateofbirth, email);

            var result = response as BadRequestObjectResult;
            result.Should().NotBeNull();

            var errorList = result?.Value as List<ValidationFailure>;
            errorList?.Count.Should().Be(1);
            errorList?[0].Should().Be(new ValidationFailure
            {
                PropertyName = "name",
                ErrorMessage = "error"
            });

            Assert.AreEqual(StatusCodes.Status400BadRequest, result!.StatusCode);
        }
    }
}