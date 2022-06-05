namespace WorkOrder.Online.Models
{
    public class OrganizationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public int NbrUsers { get; set; }
        public string Notes { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public int NbrActiveUsers { get; set; }
        public int WorkOrderCount { get; set; }
        public int EmailCount { get; set; }
    }
}
