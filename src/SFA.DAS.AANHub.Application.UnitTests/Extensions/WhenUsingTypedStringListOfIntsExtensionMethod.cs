
using FluentAssertions;
using SFA.DAS.AANHub.Application.Extensions;


namespace SFA.DAS.AANHub.Api.UnitTests.Application.Extensions
{
    public class WhenUsingTypedStringListOfIntsExtensionMethod
    {
        [Fact]
        public void And_PassingValidStringIntList_Then_ReturnEnumerable()
        {
            var input = "1,2,3";
            var expected = new List<Int64>(new Int64[] { 1, 2, 3 });
            var output = input.ToIntList(",");

            output.Should().BeAssignableTo<IEnumerable<Int64>>();
            output.Should().NotBeNullOrEmpty();
            output.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void And_PassingEmptyStringIntList_Then_ReturnEmptyEnumerable()
        {
            var input = "";
            var expected = Enumerable.Empty<Int64>();
            var output = input.ToIntList(",");

            output.Should().BeAssignableTo<IEnumerable<Int64>>();
            output.Should().NotBeNull();
            output.Should().BeEmpty();
            output.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void And_PassingStringIntListWithInvalidElements_Then_ReturnEnumerableWithValidElements()
        {
            var input = "1,2,a,3,4,b,c";
            var expected = new List<Int64>(new Int64[] { 1, 2, 3, 4 });
            var output = input.ToIntList(",");

            output.Should().BeAssignableTo<IEnumerable<Int64>>();
            output.Should().NotBeNullOrEmpty();
            output.Should().BeEquivalentTo(expected);
        }
    }
}
