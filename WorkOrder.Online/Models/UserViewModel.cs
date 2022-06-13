using System.Text;

namespace WorkOrder.Online.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName => $"{FirstName} {LastName}";
        public bool LockedOut { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cellphone { get; set; }
        public IEnumerable<string> Roles { get; set; }
 
        public string Claims { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }


        public int StartUpScreenId { get; set; }

        public string OrganizationToDisplay
        {
            get
            {
                if (Roles.Any(p => p == "SuperAdmin"))
                {
                    return "All";
                }

                return string.IsNullOrWhiteSpace(OrganizationName) ? "N/A" : OrganizationName;
            }
        }

        public string RolesToDisplay
        {

            get
            {
                var builder = new StringBuilder();

                if (!Roles.Any()) return string.Empty;

                Roles.ToList().ForEach(c =>
                {
                    builder.Append(c);
                    builder.Append("<br />");
                });

                return builder.Remove(builder.Length - 6, 6).ToString();
            }
        }

    }
}
