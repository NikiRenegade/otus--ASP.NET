using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Mappers;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<PromoCode> _promoCodeRepository;

        public PromocodesController(ICustomerRepository customerRepository, IRepository<Preference> preferenceRepository, IRepository<PromoCode> promoCodeRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
            _promoCodeRepository = promoCodeRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromoCodesAsync()
        {
            try
            {
                var promoCodes = await _promoCodeRepository.GetAllAsync();
                var promoCodesModelList = promoCodes.Select(x =>
                x.ToPromoCodeShortResponse()).ToList();
                return Ok(promoCodesModelList);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            try
            {
                var preference = (await _preferenceRepository.GetAllAsync()).FirstOrDefault(x => x.Name == request.Preference);
                if (preference == null)
                {
                    return NotFound();
                }

                var promoCode = request.ToPromoCode(preference);
                promoCode.Id = Guid.NewGuid();
                var newPromoCode = _promoCodeRepository.CreateAsync(promoCode);
                var customers = await _customerRepository.GetAllCustomersWithPreferencesAsync();

                var customersWithPreference = customers.Where(x =>
                    x.CustomerPreferences != null && x.CustomerPreferences
                        .Select(x => x.Preference).Contains(preference));
                foreach (var customer in customersWithPreference)
                {
                    customer.PromoCodes.Add(promoCode);
                    await _customerRepository.UpdateAsync(customer);

                }
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}