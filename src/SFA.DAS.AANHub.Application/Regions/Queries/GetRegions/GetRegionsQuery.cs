using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQuery : IRequest<ValidatedResponse<GetRegionsQueryResult>>
    {
    }
}