namespace WorkOrder.Online.Models
{
    public class ProjectCategoryViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Description_En { get; set; }
        public string Description_Fr { get; set; }
        public int CategoryId { get; set; }
        public decimal Hours { get; set; }
        public int sequence { get; set; }

    }
}
