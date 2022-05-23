using WorkOrder.Online.Models;

namespace WorkOrder.Online.Areas.Admin.Models
{
    public class RoleListViewModel : BaseViewModel
    {
        public IEnumerable<RoleViewModel> Roles { get; set; }
    }
}
