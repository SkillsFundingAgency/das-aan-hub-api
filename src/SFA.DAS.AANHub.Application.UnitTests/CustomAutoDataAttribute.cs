using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests
{
    public class CustomAutoDataAttribute() : AutoDataAttribute(CreateFixture)
    {
        private static IFixture CreateFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            // Customize fixture to ignore circular references
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }
}
