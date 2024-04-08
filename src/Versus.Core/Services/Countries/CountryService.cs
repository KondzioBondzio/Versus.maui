
namespace Versus.Core.Services.Countries
{
    public class CountryService
    {
        public Task<List<Country>> GetCountriesAsync()
        {
            // Tutaj możesz zastąpić to prawdziwym źródłem danych
            var countries = new List<Country>
        {
            new Country { Code = "PL", Name = "Poland" },
            new Country { Code = "US", Name = "United States" },
            // Dodaj więcej krajów zgodnie z potrzebami
        };

            return Task.FromResult(countries);
        }
    }
}
