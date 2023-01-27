namespace SFA.DAS.AANHub.Api.Common
{
    public class BaseRequestDetails
    {
        public string ActionName { get; set; } = null!;
        public string ControllerName { get; set; } = null!;
        public string? GetId { get; set; }
        public RouteValueDictionary? GetParameters { get; set; }
    }
}