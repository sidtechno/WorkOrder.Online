namespace WorkOrder.Online.Data.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string ProjectNo { get; set; }
        public string Description { get; set; }
        public int OrganizationId { get; set; }
        public int CustomerId { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<ProjectCategoryModel> ProjectsCategories { get; set; }

    }
}
