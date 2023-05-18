using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Application.Common;

[ExcludeFromCodeCoverage]
public record PutCommandResult(bool NewResourceCreated);
