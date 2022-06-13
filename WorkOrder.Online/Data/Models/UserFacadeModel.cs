namespace WorkOrder.Online.Data.Models
{
    public class UserFacadeModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int OrganizationId { get; set; }
        public int StartUpScreenId { get; set; }
        public string OrganizationName { get; set; }
        public bool LockedOut { get; set; }
        public string Roles { get; set; }
        public string UserName { get; set; }
        public string Cellphone { get; set; }
    }
}
