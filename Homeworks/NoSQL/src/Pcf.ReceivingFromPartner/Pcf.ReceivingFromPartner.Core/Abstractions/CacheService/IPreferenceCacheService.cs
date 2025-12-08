using System.Collections.Generic;
using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.CacheService;

public interface IPreferenceCacheService
{
    Task<IEnumerable<Preference>> GetPreferencesAsync();
}