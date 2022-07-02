namespace WorkOrder.Online.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Description_Fr { get; set; }
        public string Description_En { get; set; }
        public decimal Cost { get; set; }
        public decimal Retail { get; set; }
        public int OrganizationId { get; set; }
    }
}
