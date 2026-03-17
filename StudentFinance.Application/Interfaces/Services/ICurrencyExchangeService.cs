using StudentFinance.Domain.Enums;

namespace StudentFinance.Application.Interfaces.Services
{
    public interface ICurrencyExchangeService
    {
        // Function to retrieve the current exchange rate from the student's currency to the family's currency
        Task<decimal> GetExchangeRateAsync(Currency localCurrency, Currency familyBaseCurrency, CancellationToken cancellationToken);
    }
}
