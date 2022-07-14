namespace WorkOrder.Online.Data.Models
{
    public class ProjectCategoryModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int CategoryId { get; set; }
        public string Description_En { get; set; }
        public string Description_Fr { get; set; }
        public decimal Hours { get; set; }
        public int sequence { get; set; }

    }
}
