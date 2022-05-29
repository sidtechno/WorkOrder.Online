namespace WorkOrder.Online.Models
{
    public class OrganizationListViewModel : BaseViewModel
    {
        public IEnumerable<OrganizationViewModel> Organizations { get; set; }
    }
}
