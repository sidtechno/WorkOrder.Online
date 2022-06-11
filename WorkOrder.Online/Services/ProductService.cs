using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductFactory _productFactory;

        public ProductService(IProductFactory productFactory)
        {
            _productFactory = productFactory;
        }

        public async Task<IEnumerable<ProductViewModel>> GetProducts(int organizationId)
        {
            var products = await _productFactory.GetProducts(organizationId);
            return products.Adapt<IEnumerable<ProductViewModel>>();
        }

        public async Task<int> Create(ProductViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<ProductModel>();
                var productId = await _productFactory.Create(factoryModel);

                return productId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Update(ProductViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<ProductModel>();
                var productId = await _productFactory.Update(factoryModel);

                return productId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int productId)
        {
            return await _productFactory.Delete(productId);
        }
    }
}
