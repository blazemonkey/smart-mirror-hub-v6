using SmartMirrorHubV6.Shared.Components.Base;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Shared.Components.Data.Currency;

public class OpenExchangeRoot
{
    [JsonPropertyName("disclaimer")]
    public string Disclaimer { get; set; }

    [JsonPropertyName("license")]
    public string License { get; set; }

    [JsonPropertyName("timestamp")]
    public int TimestampUnix { get; set; }

    [JsonPropertyName("base")]
    public string Base { get; set; }

    [JsonPropertyName("rates")]
    public OpenExchangeRate Rates { get; set; }
}

public class OpenExchangeRate
{
    [Description("UAE Dirham")]
    public double AED { get; set; }
    [Description("Afghani")]
    public double AFN { get; set; }
    [Description("Lek")]
    public double ALL { get; set; }
    [Description("Armenian Dram")]
    public double AMD { get; set; }
    [Description("Netherlands Antillean Guilder")]
    public double ANG { get; set; }
    [Description("Kwanza")]
    public double AOA { get; set; }
    [Description("Argentine Peso")]
    public double ARS { get; set; }
    [Description("Australian Dollar")]
    public double AUD { get; set; }
    [Description("Aruban Florin")]
    public double AWG { get; set; }
    [Description("Azerbaijanian Manat")]
    public double AZN { get; set; }
    [Description("Convertible Mark")]
    public double BAM { get; set; }
    [Description("Barbados Dollar")]
    public double BBD { get; set; }
    [Description("Taka")]
    public double BDT { get; set; }
    [Description("Bulgarian Lev")]
    public double BGN { get; set; }
    [Description("Bahraini Dinar")]
    public double BHD { get; set; }
    [Description("Burundi Franc")]
    public double BIF { get; set; }
    [Description("Bermudian Dollar")]
    public double BMD { get; set; }
    [Description("Brunei Dollar")]
    public double BND { get; set; }
    [Description("Boliviano")]
    public double BOB { get; set; }
    [Description("Brazilian Real")]
    public double BRL { get; set; }
    [Description("Bahamian Dollar")]
    public double BSD { get; set; }
    [Description("Bitcoin")]
    public double BTC { get; set; }
    [Description("Ngultrum")]
    public double BTN { get; set; }
    [Description("Pula")]
    public double BWP { get; set; }
    [Description("Belarussian Ruble")]
    public double BYN { get; set; }
    [Description("Belize Dollar")]
    public double BZD { get; set; }
    [Description("Canadian Dollar")]
    public double CAD { get; set; }
    [Description("Congolese Franc")]
    public double CDF { get; set; }
    [Description("Swiss Franc")]
    public double CHF { get; set; }
    [Description("Unidad de Fomento")]
    public double CLF { get; set; }
    [Description("Chilean Peso")]
    public double CLP { get; set; }
    [Description("Yuan Renminbi (CNH)")]
    public double CNH { get; set; }
    [Description("Yuan Renminbi (CNY)")]
    public double CNY { get; set; }
    [Description("Colombian Peso")]
    public double COP { get; set; }
    [Description("Costa Rican Colon")]
    public double CRC { get; set; }
    [Description("Peso Convertible")]
    public double CUC { get; set; }
    [Description("Cuban Peso")]
    public double CUP { get; set; }
    [Description("Cabo Verde Escudo")]
    public double CVE { get; set; }
    [Description("Czech Koruna")]
    public double CZK { get; set; }
    [Description("Djibouti Franc")]
    public double DJF { get; set; }
    [Description("Danish Krone")]
    public double DKK { get; set; }
    [Description("Dominican Peso")]
    public double DOP { get; set; }
    [Description("Algerian Dinar")]
    public double DZD { get; set; }
    [Description("Egyptian Pound")]
    public double EGP { get; set; }
    [Description("Nakfa")]
    public double ERN { get; set; }
    [Description("Ethiopian Birr")]
    public double ETB { get; set; }
    [Description("Euro")]
    public double EUR { get; set; }
    [Description("Fiji Dollar")]
    public double FJD { get; set; }
    [Description("Falkland Islands Pound")]
    public double FKP { get; set; }
    [Description("Pound Sterling")]
    public double GBP { get; set; }
    [Description("Lari")]
    public double GEL { get; set; }
    [Description("Guernsey Pound")]
    public double GGP { get; set; }
    [Description("Ghana Cedi")]
    public double GHS { get; set; }
    [Description("Gibraltar Pound")]
    public double GIP { get; set; }
    [Description("Dalasi")]
    public double GMD { get; set; }
    [Description("Guinea Franc")]
    public double GNF { get; set; }
    [Description("Quetzal")]
    public double GTQ { get; set; }
    [Description("Guyana Dollar")]
    public double GYD { get; set; }
    [Description("Hong Kong Dollar")]
    public double HKD { get; set; }
    [Description("Lempira")]
    public double HNL { get; set; }
    [Description("Kuna")]
    public double HRK { get; set; }
    [Description("Gourde")]
    public double HTG { get; set; }
    [Description("Forint")]
    public double HUF { get; set; }
    [Description("Rupiah")]
    public double IDR { get; set; }
    [Description("New Israeli Sheqel")]
    public double ILS { get; set; }
    [Description("Isle of Man Pound")]
    public double IMP { get; set; }
    [Description("Indian Rupee")]
    public double INR { get; set; }
    [Description("Iraqi Dinar")]
    public double IQD { get; set; }
    [Description("Iranian Rial")]
    public double IRR { get; set; }
    [Description("Iceland Krona")]
    public double ISK { get; set; }
    [Description("Jersey Pound")]
    public double JEP { get; set; }
    [Description("Jamaican Dollar")]
    public double JMD { get; set; }
    [Description("Jordanian Dinar")]
    public double JOD { get; set; }
    [Description("Yen")]
    public double JPY { get; set; }
    [Description("Kenyan Shilling")]
    public double KES { get; set; }
    [Description("Som")]
    public double KGS { get; set; }
    [Description("Riel")]
    public double KHR { get; set; }
    [Description("Comoro Franc")]
    public double KMF { get; set; }
    [Description("North Korean Won")]
    public double KPW { get; set; }
    [Description("Won")]
    public double KRW { get; set; }
    [Description("Kuwaiti Dinar")]
    public double KWD { get; set; }
    [Description("Cayman Islands Dollar")]
    public double KYD { get; set; }
    [Description("Tenge")]
    public double KZT { get; set; }
    [Description("Kip")]
    public double LAK { get; set; }
    [Description("Lebanese Pound")]
    public double LBP { get; set; }
    [Description("Sri Lanka Rupee")]
    public double LKR { get; set; }
    [Description("Liberian Dollar")]
    public double LRD { get; set; }
    [Description("Loti")]
    public double LSL { get; set; }
    [Description("Libyan Dinar")]
    public double LYD { get; set; }
    [Description("Moroccan Dirham")]
    public double MAD { get; set; }
    [Description("Moldovan Leu")]
    public double MDL { get; set; }
    [Description("Malagasy Ariary")]
    public double MGA { get; set; }
    [Description("Denar")]
    public double MKD { get; set; }
    [Description("Kyat")]
    public double MMK { get; set; }
    [Description("Tugrik")]
    public double MNT { get; set; }
    [Description("Pataca")]
    public double MOP { get; set; }
    [Description("Ouguiya")]
    public double MRU { get; set; }
    [Description("Mauritius Rupee")]
    public double MUR { get; set; }
    [Description("Rufiyaa")]
    public double MVR { get; set; }
    [Description("Kwacha")]
    public double MWK { get; set; }
    [Description("Mexican Peso")]
    public double MXN { get; set; }
    [Description("Malaysian Ringgit")]
    public double MYR { get; set; }
    [Description("Mozambique Metical")]
    public double MZN { get; set; }
    [Description("Namibia Dollar")]
    public double NAD { get; set; }
    [Description("Naira")]
    public double NGN { get; set; }
    [Description("Cordoba Oro")]
    public double NIO { get; set; }
    [Description("Norwegian Krone")]
    public double NOK { get; set; }
    [Description("Nepalese Rupee")]
    public double NPR { get; set; }
    [Description("New Zealand Dollar")]
    public double NZD { get; set; }
    [Description("Rial Omani")]
    public double OMR { get; set; }
    [Description("Balboa")]
    public double PAB { get; set; }
    [Description("Nuevo Sol")]
    public double PEN { get; set; }
    [Description("Kina")]
    public double PGK { get; set; }
    [Description("Philippine Peso")]
    public double PHP { get; set; }
    [Description("Pakistan Rupee")]
    public double PKR { get; set; }
    [Description("Zloty")]
    public double PLN { get; set; }
    [Description("Guarani")]
    public double PYG { get; set; }
    [Description("Qatari Rial")]
    public double QAR { get; set; }
    [Description("Romanian Leu")]
    public double RON { get; set; }
    [Description("Serbian Dinar")]
    public double RSD { get; set; }
    [Description("Russian Ruble")]
    public double RUB { get; set; }
    [Description("Rwanda Franc")]
    public double RWF { get; set; }
    [Description("Saudi Riyal")]
    public double SAR { get; set; }
    [Description("Solomon Islands Dollar")]
    public double SBD { get; set; }
    [Description("Seychelles Rupee")]
    public double SCR { get; set; }
    [Description("Sudanese Pound")]
    public double SDG { get; set; }
    [Description("Swedish Krona")]
    public double SEK { get; set; }
    [Description("Singapore Dollar")]
    public double SGD { get; set; }
    [Description("Saint Helena Pound")]
    public double SHP { get; set; }
    [Description("Leone")]
    public double SLL { get; set; }
    [Description("Somali Shilling")]
    public double SOS { get; set; }
    [Description("Surinam Dollar")]
    public double SRD { get; set; }
    [Description("South Sudanese Pound")]
    public double SSP { get; set; }
    [Description("Dobra")]
    public double STN { get; set; }
    [Description("El Salvador Colon")]
    public double SVC { get; set; }
    [Description("Syrian Pound")]
    public double SYP { get; set; }
    [Description("Lilangeni")]
    public double SZL { get; set; }
    [Description("Baht")]
    public double THB { get; set; }
    [Description("Somoni")]
    public double TJS { get; set; }
    [Description("Turkmenistan New Manat")]
    public double TMT { get; set; }
    [Description("Tunisian Dinar")]
    public double TND { get; set; }
    [Description("Pa’anga")]
    public double TOP { get; set; }
    [Description("Turkish Lira")]
    public double TRY { get; set; }
    [Description("Trinidad and Tobago Dollar")]
    public double TTD { get; set; }
    [Description("New Taiwan Dollar")]
    public double TWD { get; set; }
    [Description("Tanzanian Shilling")]
    public double TZS { get; set; }
    [Description("Hryvnia")]
    public double UAH { get; set; }
    [Description("Uganda Shilling	")]
    public double UGX { get; set; }
    [Description("US Dollar")]
    public double USD { get; set; }
    [Description("Peso Uruguayo")]
    public double UYU { get; set; }
    [Description("Uzbekistan Sum")]
    public double UZS { get; set; }
    [Description("Bolivar")]
    public double VES { get; set; }
    [Description("Dong")]
    public double VND { get; set; }
    [Description("Vatu")]
    public double VUV { get; set; }
    [Description("Tala")]
    public double WST { get; set; }
    [Description("CFA Franc BEAC")]
    public double XAF { get; set; }
    [Description("East Caribbean Dollar")]
    public double XCD { get; set; }
    [Description("SDR (Special Drawing Right)")]
    public double XDR { get; set; }
    [Description("CFA Franc BCEAO")]
    public double XOF { get; set; }
    [Description("CFP Franc")]
    public double XPT { get; set; }
    [Description("Yemeni Rial")]
    public double YER { get; set; }
    [Description("Rand")]
    public double ZAR { get; set; }
    [Description("Zambian Kwacha")]
    public double ZMW { get; set; }
    [Description("Zimbabwe Dollar")]
    public double ZWL { get; set; }
}

public class OpenExchangeRateResponse
{
    public string Code { get; set; }
    public string Name { get; set; }
    public double Rate { get; set; }
}

public class OpenExchangeResponse : ComponentResponse
{
    public string Base { get; set; }
    public DateTime PublishTimeUtc { get; set; }
    public List<OpenExchangeRateResponse> Rates { get; set; }

    public OpenExchangeResponse()
    {
        Rates = new List<OpenExchangeRateResponse>();
    }

    public static explicit operator OpenExchangeResponse(OpenExchangeRoot currency)
    {
        var response = new OpenExchangeResponse();
        response.PublishTimeUtc = DateTimeOffset.FromUnixTimeSeconds(currency.TimestampUnix).UtcDateTime;

        var rateProperties = typeof(OpenExchangeRate).GetProperties();
        foreach (var rp in rateProperties)
        {
            double rate = (double)rp.GetValue(currency.Rates);
            var attribute = rp.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null)
                continue;

            var currencyRate = new OpenExchangeRateResponse()
            {
                Code = rp.Name,
                Name = attribute.Description,
                Rate = rate
            };

            response.Rates.Add(currencyRate);
        }

        return response;
    }
}
