using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberByEmail;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries;

public class GetMemberByEmailQueryValidatorTests
{
    [Test, RecursiveMoqAutoData]
    public async Task MissingUser_Fails_Validation(Member member)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMemberByEmail(member.Email))
            .ReturnsAsync(member);

        var query = new GetMemberByEmailQuery(string.Empty);

        var sut = new GetMemberByEmailQueryValidator();

        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage(GetMemberByEmailQueryValidator.EmailMissingMessage);
    }
}
