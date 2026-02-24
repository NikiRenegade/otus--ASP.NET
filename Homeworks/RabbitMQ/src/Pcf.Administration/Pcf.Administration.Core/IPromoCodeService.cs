using System;
using System.Threading.Tasks;

namespace Pcf.Administration.Core;

public interface IPromoCodeService
{
    Task UpdateAppliedPromocodesAsync(Guid id);
}