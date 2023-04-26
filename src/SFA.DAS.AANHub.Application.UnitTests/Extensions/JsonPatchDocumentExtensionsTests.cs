using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Extensions;

namespace SFA.DAS.AANHub.Application.UnitTests.Extensions;

public class JsonPatchDocumentExtensionsTests
{
    [TestCase("name")]
    [TestCase("Name")]
    [TestCase("/name")]
    public void GetReplacementValue_AcceptsPathInAnyFormat(string path)
    {
        const string expected = "John";
        JsonPatchDocument<Person> doc = new();
        doc.Replace(p => p.Name, expected);

        var actual = doc.GetReplacementValue(path);

        actual.Should().Be(expected);
    }

    [Test]
    public void GetReplacementValueAsInt_ReturnsInt()
    {
        const int expected = 1;
        JsonPatchDocument<Person> doc = new();
        doc.Replace(p => p.Age, expected);

        var actual = doc.GetReplacementValueAsInt("age");

        actual.Should().Be(expected);
    }

    [TestCase("address", false)]
    [TestCase("name", true)]
    public void HasValue_TrueIfPathExists(string value, bool expected)
    {
        JsonPatchDocument<Person> doc = new();
        doc.Replace(p => p.Name, "John");

        var actual = doc.HasValue(value);

        actual.Should().Be(expected);
    }

    [Test]
    public void PatchOperationsFieldList_ReturnsPathsInLowerCase()
    {
        JsonPatchDocument<Person> doc = new();
        doc.Replace(p => p.Name, "John");
        doc.Replace(p => p.Age, 1);

        var actual = doc.PatchOperationsFieldList();

        actual.Should().BeSubsetOf(new[] { "age", "name" });
    }

    record Person(string Name, int Age);
}
