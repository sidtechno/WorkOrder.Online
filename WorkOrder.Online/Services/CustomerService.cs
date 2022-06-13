using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerFactory _customerFactory;

        public CustomerService(ICustomerFactory customerFactory)
        {
            _customerFactory = customerFactory;
        }

        public async Task<IEnumerable<CustomerViewModel>> GetCustomers(int organizationId)
        {
            var customers = await _customerFactory.GetCustomers(organizationId);
            return customers.Adapt<IEnumerable<CustomerViewModel>>();
        }

        public async Task<int> Create(CustomerViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<CustomerModel>();
                var customerId = await _customerFactory.Create(factoryModel);

                return customerId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Update(CustomerViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<CustomerModel>();
                var customerId = await _customerFactory.Update(factoryModel);

                return customerId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int customerId)
        {
            return await _customerFactory.Delete(customerId);
        }
    }
}
