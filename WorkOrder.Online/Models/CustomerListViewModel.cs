namespace WorkOrder.Online.Models
{
    public class CustomerListViewModel : BaseViewModel
    {
        public IEnumerable<CustomerViewModel> Customers { get; set; }
        public OrganizationSelectorViewModel OrganizationSelector { get; set; }

    }
}
