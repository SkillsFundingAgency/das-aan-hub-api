
namespace SFA.DAS.AAN.Hub.Api.Requests
{
    public class CreateMemberRequest
    {
        public string id { get; set; }
        public DateTime joined { get; set; }
        public int? region { get; set; }
        public string information { get; set; }
        public string organisation { get; set; }
    }
}
