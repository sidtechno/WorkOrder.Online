namespace WorkOrder.Online.Models
{
    public class WorkOrderListViewModel : BaseViewModel
    {
        public IEnumerable<WorkOrderViewModel> WorkOrders { get; set; }
        public OrganizationSelectorViewModel OrganizationSelector { get; set; }
        public ProjectListViewModel ProjectListViewModel { get; set; }  
    }
}
