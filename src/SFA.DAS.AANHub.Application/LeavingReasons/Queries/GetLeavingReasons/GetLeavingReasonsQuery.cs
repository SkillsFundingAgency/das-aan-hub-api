using MediatR;

namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;
public record GetLeavingReasonsQuery : IRequest<IEnumerable<LeavingCategory>>;
