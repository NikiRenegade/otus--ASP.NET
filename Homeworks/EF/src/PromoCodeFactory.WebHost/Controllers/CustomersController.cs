using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Mappers;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomersController(ICustomerRepository customerRepository, IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить короткую информацию по всем клиентам
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                var customersModelList = customers.Select(x =>
                    new CustomerShortResponse()
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email
                    }).ToList();
                return Ok(customersModelList);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Получить полную информацию по клиенту
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerWithPreferenceByIdAsync(id);
                if (customer == null)
                {
                    NotFound();
                }
                var customerModel = customer.ToCustomerResponse();
                return Ok(customerModel); ;
            }
            catch
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Создать клиента
        /// </summary>
        /// <param name="request">Модель создания клиента</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CustomerShortResponse>> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            try
            {
                var preferences = await _preferenceRepository.GetAllAsync();
                var customerPreferences = preferences.Where(p => request.PreferenceIds.Contains(p.Id));

                var customer = request.ToCustomer(customerPreferences);
                var createdCustomer = await _customerRepository.CreateAsync(customer);
                return CreatedAtAction(nameof(GetCustomerAsync), new { id = createdCustomer.Id }, createdCustomer.ToCustomerShortResponse());
            }
            catch
            {
                return BadRequest();
            }


        }

        /// <summary>
        /// Редактировать клиента
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <param name="request">Модель редактирования клиента</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerShortResponse>> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetByIdAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound();
                }
                var preferences = await _preferenceRepository.GetAllAsync();
                var customerPreferences = preferences.Where(p => request.PreferenceIds.Contains(p.Id));
                var updatedCustomer = request.ToCustomer(customerPreferences);
                updatedCustomer.Id = id;
                var resultCustomer = await _customerRepository.UpdateAsync(updatedCustomer);
                return resultCustomer.ToCustomerShortResponse();
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCustomer(Guid id)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }
            var result = await _customerRepository.DeleteAsync(id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}