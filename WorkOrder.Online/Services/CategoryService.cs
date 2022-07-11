using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryFactory _categoryFactory;

        public CategoryService(ICategoryFactory categoryFactory)
        {
            _categoryFactory = categoryFactory;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategories(int organizationId)
        {
            var categories = await _categoryFactory.GetCategories(organizationId);
            return categories.Adapt<IEnumerable<CategoryViewModel>>();
        }

        public async Task<int> Create(CategoryViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<CategoryModel>();
                var productId = await _categoryFactory.Create(factoryModel);

                return productId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Update(CategoryViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<CategoryModel>();
                var productId = await _categoryFactory.Update(factoryModel);

                return productId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int categoryId)
        {
            return await _categoryFactory.Delete(categoryId);
        }

        public async Task<IEnumerable<SelectListItem>> GetCategorySelectList(int organizationId, string language)
        {
            var categories = await _categoryFactory.GetCategories(organizationId);

            return categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = language.ToLower() == "fr" ? x.Description_Fr : x.Description_En
            }).OrderBy(o => o.Text);
        }
    }
}
