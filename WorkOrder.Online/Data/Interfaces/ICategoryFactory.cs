using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface ICategoryFactory
    {
        Task<IEnumerable<CategoryModel>> GetCategories(int organizationId);
        Task<int> Create(CategoryModel model);
        Task<int> Update(CategoryModel model);
        Task<int> Delete(int categoryId);

    }
}
