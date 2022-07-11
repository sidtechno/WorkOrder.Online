using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkOrder.Online.Models
{
    public class CategorySelectorViewModel
    {
        public string Name { get; set; }
        public int SelectedCategoryId { get; set; }

        public bool disabled { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string UniqueId
        {
            get
            {
                return Guid.NewGuid().ToString("N");
            }
        }
    }
}
