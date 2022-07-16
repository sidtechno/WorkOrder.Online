namespace WorkOrder.Online.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string ProjectNo { get; set; }
        public string Description { get; set; }
        public int OrganizationId { get; set; }
        public int NbWorkOrder { get; set; }
        public int CustomerId { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<ProjectCategoryViewModel> ProjectsCategories { get; set; }
    }
}
