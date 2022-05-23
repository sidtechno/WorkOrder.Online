using System.Security.Claims;

namespace WorkOrder.Online.Models
{
    public class UserCredentials
    {
        public UserCredentials() { }

        public string UserId { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public string FullName { get; set; }
        public string Language { get; set; }
    }
}
