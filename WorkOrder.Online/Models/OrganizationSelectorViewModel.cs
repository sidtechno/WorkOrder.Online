using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkOrder.Online.Models
{
    public class OrganizationSelectorViewModel
    {
        public string Name { get; set; }
        public int SelectedOrganizationId { get; set; }

        public bool disabled { get; set; }
        public IEnumerable<SelectListItem> Organizations { get; set; }
        public string UniqueId
        {
            get
            {
                return Guid.NewGuid().ToString("N");
            }
        }
    }
}
