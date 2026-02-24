using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Abstractions;
using Pcf.GivingToCustomer.Core.Mappers;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.Core;

public class PromoCodeService: IPromoCodeService
{
    private readonly IRepository<PromoCode> _promoCodesRepository;
    private readonly IRepository<Preference> _preferencesRepository;
    private readonly IRepository<Customer> _customersRepository;

    public PromoCodeService(IRepository<PromoCode> promoCodesRepository,
        IRepository<Preference> preferencesRepository, IRepository<Customer> customersRepository)
    {
        _promoCodesRepository = promoCodesRepository;
        _preferencesRepository = preferencesRepository;
        _customersRepository = customersRepository;
    }

    public async Task GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeReq req)
    {
        // Получаем предпочтение
        var preference = await _preferencesRepository.GetByIdAsync(req.PreferenceId);
        if (preference == null)
        {
            throw new ApplicationException($"PromoCode не найден.");
        }
        
        var customers = await _customersRepository
            .GetWhere(d => d.Preferences.Any(x => x.Preference.Id == preference.Id));
        
        PromoCode promoCode = PromoCodeMapper.MapFromModel(req, preference, customers);
        await _promoCodesRepository.AddAsync(promoCode);
    }
}