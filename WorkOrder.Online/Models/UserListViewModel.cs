namespace WorkOrder.Online.Models
{
    public class UserListViewModel : BaseViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public OrganizationSelectorViewModel OrganizationSelector { get; set; }
        public int RemainingUsers { get; set; }
    }
}
