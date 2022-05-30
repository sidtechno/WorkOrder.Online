using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkOrder.Online.Models
{
    public class LanguageSelectorViewModel
    {
        public string Name { get; set; }
        public string SelectedLanguageCode { get; set; }

        public bool disabled { get; set; }
        public IEnumerable<SelectListItem> Languages { get; set; }
        public string UniqueId
        {
            get
            {
                return Guid.NewGuid().ToString("N");
            }
        }
    }
}
