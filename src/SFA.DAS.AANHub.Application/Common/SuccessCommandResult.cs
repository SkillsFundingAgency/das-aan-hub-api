using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Application.Common;

[ExcludeFromCodeCoverage]
public record SuccessCommandResult(bool IsSuccess = true);
