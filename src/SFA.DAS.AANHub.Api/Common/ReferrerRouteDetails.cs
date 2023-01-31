namespace SFA.DAS.AANHub.Api.Common
{
    public class ReferrerRouteDetails
    {
        public ReferrerRouteDetails(string? actionName, string? controllerName, RouteValueDictionary routeParameters)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            RouteParameters = routeParameters;
        }

        public string? ActionName { get; }
        public string? ControllerName { get; }
        public RouteValueDictionary? RouteParameters { get; }
    }
}