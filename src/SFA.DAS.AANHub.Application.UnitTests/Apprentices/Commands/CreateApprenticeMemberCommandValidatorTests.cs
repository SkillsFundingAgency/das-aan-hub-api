using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands
{
    [TestFixture]
    public class CreateApprenticeMemberCommandValidatorTests
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;
        private readonly Mock<IRegionsReadRepository> _regionsReadRepository;
        private readonly Mock<IApprenticesReadRepository> _apprenticesReadRepository;

        public CreateApprenticeMemberCommandValidatorTests()
        {
            _memberReadRepository = new Mock<IMembersReadRepository>();
            _regionsReadRepository = new Mock<IRegionsReadRepository>();
            _apprenticesReadRepository = new Mock<IApprenticesReadRepository>();
        }

        [TestCase("684b9829-d882-4733-938e-bcee6d6bfe81", true)]
        [TestCase(null, false)]
        [TestCase("00000000-0000-0000-0000-000000000000", false)]
        public async Task Validates_ApprenticeId_NotNull(Guid apprenticeId, bool isValid)
        {
            var command = new CreateApprenticeMemberCommand
            {
                ApprenticeId = apprenticeId
            };

            var sut = new CreateApprenticeMemberCommandValidator(_memberReadRepository.Object,
                _regionsReadRepository.Object,
                _apprenticesReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.ApprenticeId);
            else
                result.ShouldHaveValidationErrorFor(c => c.ApprenticeId);
        }
    }
}