using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<SelectListItem>> GetCategorySelectList(int organizationId, string language);
        Task<IEnumerable<CategoryViewModel>> GetCategories(int organizationId);
        Task<int> Create(CategoryViewModel model);
        Task<int> Update(CategoryViewModel model);
        Task<int> Delete(int categoryId);
    }
}
