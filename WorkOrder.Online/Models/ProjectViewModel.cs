namespace WorkOrder.Online.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string ProjectNo { get; set; }
        public string Description { get; set; }
        public int OrganizationId { get; set; }
        public int NbWorkOrder { get; set; }
    }
}
