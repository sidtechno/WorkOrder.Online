namespace WorkOrder.Online.Data.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Description_Fr { get; set; }
        public string Description_En { get; set; }
        public decimal Cost { get; set; }
        public decimal Retail { get; set; }
        public int OrganizationId { get; set; }
    }
}
