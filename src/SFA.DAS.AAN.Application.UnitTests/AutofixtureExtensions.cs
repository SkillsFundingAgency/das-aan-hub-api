using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AAN.Data;
using SFA.DAS.AAN.Domain.Interfaces;

namespace SFA.DAS.AAN.Application.UnitTests
{
    public static class AutofixtureExtensions
    {
        public static IFixture AanFixture()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customize(new ApprenticeFeedbackCustomization());
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            return fixture;
        }
    }

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(AutofixtureExtensions.AanFixture)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AutoMoqInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public AutoMoqInlineAutoDataAttribute(params object[] arguments)
            : base(AutofixtureExtensions.AanFixture(), arguments)
        {
        }
    }

    public class ApprenticeFeedbackCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(new AANDataContextBuilder());
            fixture.Customizations.Add(new DateTimeHelperBuilder());
        }
    }

    public class AANDataContextBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(AanDataContext))
            {

                var connection = new SqliteConnection("Filename=:memory:");
                connection.Open();

                // These options will be used by the context instances in this test suite, including the connection opened above.
                var contextOptions = new DbContextOptionsBuilder<AanDataContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema and seed some data
                var dbContext = new AanDataContext(contextOptions);
                dbContext.Database.EnsureCreated();

                return dbContext;
            }

            return new NoSpecimen();
        }
    }

    // Handy for unit testing - make sure any IDateTimeHelper instances are "now"
    public class DateTimeHelperBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(IDateTimeHelper))
            {
                return new UtcTimeProvider();
            }

            return new NoSpecimen();
        }
    }
}
