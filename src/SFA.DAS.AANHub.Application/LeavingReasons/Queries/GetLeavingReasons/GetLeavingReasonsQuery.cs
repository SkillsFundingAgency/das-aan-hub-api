using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

[ExcludeFromCodeCoverage]
public record GetLeavingReasonsQuery : IRequest<IEnumerable<LeavingCategory>>;
