﻿namespace WorkOrder.Online.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description_Fr { get; set; }
        public string Description_En { get; set; }
        public decimal Price { get; set; }
        public bool IsFlatRate { get; set; }
        public int OrganizationId { get; set; }
    }
}
