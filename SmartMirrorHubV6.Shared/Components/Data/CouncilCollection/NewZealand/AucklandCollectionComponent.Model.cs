namespace SmartMirrorHubV6.Shared.Components.Data.CouncilCollection.NewZealand;

public class AucklandCollectionRoot
{
    public int ResultCount { get; set; }
    public string SearchText { get; set; }
    public bool RateKeyRequired { get; set; }

    public AucklandCollectionRoot()
    {
        ResultCount = 10;
        RateKeyRequired = false;
    }
}

public class AucklandCollectionKeyResponse
{
    public string ACRateAccountKey { get; set; }
    public string Address { get; set; }
    public string Suggestion { get; set; }
}
