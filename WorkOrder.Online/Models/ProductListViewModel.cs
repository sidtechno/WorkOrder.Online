namespace WorkOrder.Online.Models
{
    public class ProductListViewModel : BaseViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
        public OrganizationSelectorViewModel OrganizationSelector { get; set; }

    }
}
