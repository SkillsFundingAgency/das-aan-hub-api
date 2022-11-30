
namespace SFA.DAS.AANHub.Application.Extensions
{
    public static class TypedStringListExtension
    {
        public static IEnumerable<Guid> ToGuidList(this string? source, string separator)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrEmpty(separator))
                return Enumerable.Empty<Guid>();

            return source.Split(separator)
                         .Where(g => { Guid x; return Guid.TryParse(g, out x); })
                         .Select(g => Guid.Parse(g));
        }

        public static IEnumerable<Int64> ToIntList(this string? source, string separator)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrEmpty(separator))
                return Enumerable.Empty<Int64>();

            return source.Split(separator)
                         .Where(i => { Int64 x; return Int64.TryParse(i, out x); })
                         .Select(i => Int64.Parse(i));
        }
    }
}
