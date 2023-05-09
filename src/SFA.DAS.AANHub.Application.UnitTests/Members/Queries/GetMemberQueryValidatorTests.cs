using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries;
public class GetMemberQueryValidatorTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task MissingUser_Fails_Validation(Member member)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id))
                                 .ReturnsAsync(member);

        var query = new GetMemberQuery(Guid.Empty);

        var sut = new GetMemberQueryValidator();

        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(c => c.UserRef)
              .WithErrorCode("NotEmptyValidator");
    }
}
