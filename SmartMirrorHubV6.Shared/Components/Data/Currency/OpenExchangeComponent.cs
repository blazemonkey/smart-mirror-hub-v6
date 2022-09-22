using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.Currency;

public class OpenExchangeComponent : ApiComponent
{
    public override string Name => "Exchange Rate";
    public override string Description => "Get exchange rate data";
    public override string Author => "Open Exchange Rate";
    public override string BaseUrl => "https://openexchangerates.org/api/latest.json";
    public override ComponentCategory Category => ComponentCategory.Currency;
    public override int Interval => 60 * 60;
    public override string VoiceName => "Currency";

    #region Inputs
    [ComponentInput("Base Currency")]
    public string BaseCurrency { get; set; }
    [ComponentInput("Selected Currencies")]
    public string[] SelectedCurrencies { get; set; }
    #endregion

    protected override async Task<ComponentResponse> Get()
    {
        var result = await RestService.Instance.Get<OpenExchangeRoot>($"{BaseUrl}?app_id={AccessToken}");
        var response = (OpenExchangeResponse)result;

        var baseCurrency = response.Rates.FirstOrDefault(x => x.Code == BaseCurrency);
        if (baseCurrency == null)
            return new ComponentResponse() { Error = "Could not find Base Currency" };

        response.Base = BaseCurrency;
        response.Rates = response.Rates.Where(x => SelectedCurrencies.Contains(x.Code)).ToList();
        response.Rates.ForEach(x => x.Rate /= baseCurrency.Rate);
        return response;
    }
}
