using System.Diagnostics.CodeAnalysis;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Admins.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetAdminMemberResult
    {
        public Guid MemberId { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;

        public static implicit operator GetAdminMemberResult?(Admin? admin)
        {
            if (admin == null)
                return null;

            return new GetAdminMemberResult
            {
                Email = admin.Email,
                Name = admin.Name,
                MemberId = admin.MemberId
            };
        }
    }
}