using MediatR;

namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;
public class GetLeavingReasonsQuery : IRequest<List<LeavingCategory>>
{
}
