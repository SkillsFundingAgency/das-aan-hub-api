using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Commands.CreateMember;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.Validators
{
    [TestFixture]
    public class CreateMemberCommandBaseValidatorTests
    {
        private readonly Mock<IRegionsReadRepository> _regionsReadRepository;

        private readonly List<Region> _regions = new()
        {
            new Region { Id = 1, Area = "Area1", Ordering = 1 },
            new Region { Id = 2, Area = "Area2", Ordering = 2 }
        };

        public CreateMemberCommandBaseValidatorTests() => _regionsReadRepository = new Mock<IRegionsReadRepository>();

        [TestCase(200, true)]
        [TestCase(201, false)]
        [TestCase(0, false)]
        public async Task Validates_Name_Length(int stringLength, bool isValid)
        {

            var command = new CreateMemberCommand { Name = new string('a', stringLength) };
            var sut = new CreateMemberCommandBaseValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Name);
            else
                result.ShouldHaveValidationErrorFor(c => c.Name);
        }
        [TestCase(250, true)]
        [TestCase(251, false)]
        [TestCase(0, false)]
        public async Task Validates_Information_Length(int stringLength, bool isValid)
        {
            var command = new CreateMemberCommand { Information = new string('a', stringLength) };
            var sut = new CreateMemberCommandBaseValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Information);
            else
                result.ShouldHaveValidationErrorFor(c => c.Information);
        }
        [TestCase(10, "@email.com", true)]
        [TestCase(257, "@email.com", false)]
        [TestCase(0, "", false)]
        public async Task Validates_Email_Length(int stringLength, string emailSuffix, bool isValid)
        {
            var emailString = new string('a', stringLength) + emailSuffix;
            var command = new CreateMemberCommand { Email = emailString };
            var sut = new CreateMemberCommandBaseValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Email);
            else
                result.ShouldHaveValidationErrorFor(c => c.Email);
        }
        [Test]
        [TestCase(new[] { 1, 2 }, true)]
        [TestCase(new[] { -1 }, false)]
        [TestCase(new[] { 1, -1 }, false)]
        [TestCase(new[] { 10 }, false)]
        public async Task Validates_Region_Range(int[] regions, bool isValid)
        {
            var command = new CreateMemberCommand { Regions = regions };
            var sut = new CreateMemberCommandBaseValidator(_regionsReadRepository.Object);
            _regionsReadRepository.Setup(m => m.GetAllRegions()).ReturnsAsync(_regions);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Regions);
            else
                result.ShouldHaveValidationErrorFor(c => c.Regions);
        }
    }
}
