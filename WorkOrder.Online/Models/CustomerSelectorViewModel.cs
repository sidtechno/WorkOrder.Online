using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkOrder.Online.Models
{
    public class CustomerSelectorViewModel
    {
        public string Name { get; set; }
        public int SelectedCustomerId { get; set; }

        public bool disabled { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
        public string UniqueId
        {
            get
            {
                return Guid.NewGuid().ToString("N");
            }
        }
    }
}
