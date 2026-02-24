using System.Threading;
using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Domain;
namespace Pcf.GivingToCustomer.Core.Abstractions;

public interface IPromoCodeService
{
    Task GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeReq req);
}