using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetProducts(int organizationId);
        Task<int> Create(ProductViewModel model);
        Task<int> Update(ProductViewModel model);
        Task<int> Delete(int productId);
    }
}
