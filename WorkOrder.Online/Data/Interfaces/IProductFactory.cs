using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface IProductFactory
    {
        Task<IEnumerable<ProductModel>> GetProducts(int organizationId);
        Task<int> Create(ProductModel model);
        Task<int> Update(ProductModel model);
        Task<int> Delete(int productId);

    }
}
