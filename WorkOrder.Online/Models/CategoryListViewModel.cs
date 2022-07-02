namespace WorkOrder.Online.Models
{
    public class CategoryListViewModel : BaseViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public OrganizationSelectorViewModel OrganizationSelector { get; set; }

    }
}
