namespace WorkOrder.Online.Models
{
    public class ProjectListViewModel : BaseViewModel
    {
        public IEnumerable<ProjectViewModel> Projects { get; set; }
        public OrganizationSelectorViewModel OrganizationSelector { get; set; }

    }
}
