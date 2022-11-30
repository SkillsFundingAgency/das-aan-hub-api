
using FluentAssertions;
using SFA.DAS.AANHub.Application.Extensions;


namespace SFA.DAS.AANHub.Api.UnitTests.Application.Extensions
{
    public class WhenUsingTypedStringListOfGuidsExtensionMethod
    {
        private const string str1 = "00000000-0000-0000-0000-000000000001";
        private const string str2 = "00000000-0000-0000-0000-000000000002";
        private const string str3 = "00000000-0000-0000-0000-000000000003";
        private const string str4 = "00000000-0000-0000-0000-000000000004";
        private readonly Guid guid1 = new Guid(str1);
        private readonly Guid guid2 = new Guid(str2);
        private readonly Guid guid3 = new Guid(str3);
        private readonly Guid guid4 = new Guid(str4);

        [Fact]
        public void And_PassingValidStringGuidList_Then_ReturnEnumerable()
        {
            var input = $"{str1},{str2},{str3}";
            var expected = new List<Guid>(new Guid[] { guid1, guid2, guid3 });
            var output = input.ToGuidList(",");

            output.Should().BeAssignableTo<IEnumerable<Guid>>();
            output.Should().NotBeNullOrEmpty();
            output.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void And_PassingEmptyStringGuidList_Then_ReturnEmptyEnumerable()
        {
            var input = "";
            var expected = Enumerable.Empty<Guid>();
            var output = input.ToGuidList(",");

            output.Should().BeAssignableTo<IEnumerable<Guid>>();
            output.Should().NotBeNull();
            output.Should().BeEmpty();
            output.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void And_PassingStringGuidListWithInvalidElements_Then_ReturnEnumerableWithValidElements()
        {
            var input = $"{str1},{str2},123,{str3},{str4},4x6,789";
            var expected = new List<Guid>(new Guid[] { guid1, guid2, guid3, guid4 });
            var output = input.ToGuidList(",");

            output.Should().BeAssignableTo<IEnumerable<Guid>>();
            output.Should().NotBeNullOrEmpty();
            output.Should().BeEquivalentTo(expected);
        }
    }
}
