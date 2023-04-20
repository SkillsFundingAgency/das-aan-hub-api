using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using Swashbuckle.AspNetCore.Filters;

namespace SFA.DAS.AANHub.Api.SwaggerExamples;

[ExcludeFromCodeCoverage]
public class PatchMemberExample : IExamplesProvider<JsonPatchDocument>
{
    public JsonPatchDocument GetExamples()
    {
        JsonPatchDocument doc = new();
        doc.Replace(MemberPatchFields.Email, "abc@gmail.com");
        doc.Replace(MemberPatchFields.FirstName, "John");
        doc.Replace(MemberPatchFields.LastName, "Smith");
        doc.Replace(MemberPatchFields.OrganisationName, "Apprenticeship Inc.");
        doc.Replace(MemberPatchFields.RegionId, 1);
        doc.Replace(MemberPatchFields.Status, "deleted | withdrawn");
        return doc;
    }
}
