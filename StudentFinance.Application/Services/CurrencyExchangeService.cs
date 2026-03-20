using Microsoft.Extensions.Logging;
using StudentFinance.Application.Interfaces.Services;
using StudentFinance.Domain.Enums;

namespace StudentFinance.Application.Services
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly ILogger<CurrencyExchangeService> _logger;

        public CurrencyExchangeService(ILogger<CurrencyExchangeService> logger)
        {
            _logger = logger;
        }

        public Task<decimal> GetExchangeRateAsync(
            Currency localCurrency,
            Currency familyBaseCurrency,
            CancellationToken cancellationToken)
        {
            if (localCurrency == familyBaseCurrency)
                return Task.FromResult(1m);

            // Mock rates: well change it later
            var rate = (localCurrency, familyBaseCurrency) switch
            {
                (Currency.USD, Currency.SAR) => 3.75m,
                (Currency.SAR, Currency.USD) => 0.27m,
                (Currency.RUB, Currency.SAR) => 0.04m,
                _ => 1m
            };

            _logger.LogInformation(
                "Exchange rate used: {From} -> {To} = {Rate}",
                localCurrency,
                familyBaseCurrency,
                rate);

            return Task.FromResult(rate);
        }
    }
}
