using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkOrder.Online.Models
{
    public class ProjectListViewModel : BaseViewModel
    {
        public IEnumerable<ProjectViewModel> Projects { get; set; }
        public CustomerSelectorViewModel CustomerSelector { get; set; }
        public OrganizationSelectorViewModel OrganizationSelector { get; set; }

    }
}
