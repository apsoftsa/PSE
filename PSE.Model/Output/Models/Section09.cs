using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ShortTermInvestment : IShortTermInvestment
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Isin { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ValorNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NominalAmount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty("s&pRating", NullValueHandling = NullValueHandling.Ignore)]
        public string SAndPRating { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MsciEsg { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PurchasePrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentPriceToPurchase { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PercentImpactChangeToPurchase { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PriceAtTheBeginningOfTheYear { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentPriceYTD { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PercentImpactChangeYTD { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Performance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PercentPerformance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PercentAssets { get; set; }
                      
        public ShortTermInvestment()
        {
            Description = string.Empty;
            Isin = string.Empty;
            ValorNumber = string.Empty;
            NominalAmount = string.Empty;
            Currency = string.Empty;
            SAndPRating = string.Empty;
            MsciEsg = string.Empty;
            PurchasePrice = string.Empty;
            CurrentPriceToPurchase = string.Empty;
            PercentImpactChangeToPurchase = string.Empty;
            PriceAtTheBeginningOfTheYear = string.Empty;
            CurrentPriceYTD = string.Empty;
            PercentImpactChangeYTD = string.Empty;
            Performance = string.Empty;
            PercentPerformance = string.Empty;
            PercentAssets = string.Empty;
        }

        public ShortTermInvestment(IShortTermInvestment source)
        {
            Description = source.Description;
            Isin = source.Isin;
            ValorNumber = source.ValorNumber;
            NominalAmount = source.NominalAmount;
            Currency = source.Currency;
            SAndPRating = source.SAndPRating;
            MsciEsg = source.MsciEsg;
            PurchasePrice = source.PurchasePrice;
            CurrentPriceToPurchase = source.CurrentPriceToPurchase;
            PercentImpactChangeToPurchase = source.PercentImpactChangeToPurchase;
            PriceAtTheBeginningOfTheYear = source.PriceAtTheBeginningOfTheYear;
            CurrentPriceYTD = source.CurrentPriceYTD;
            PercentImpactChangeYTD = source.PercentImpactChangeYTD;
            Performance = source.Performance;
            PercentPerformance = source.PercentPerformance;
            PercentAssets = source.PercentAssets;
        }

    }    

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section9Content : ISection9Content
    {

        [JsonProperty(propertyName: "investment")]
        public IList<IShortTermInvestment> Investments { get; set; }

        public Section9Content()
        {
            Investments = new List<IShortTermInvestment>();
        }

        public Section9Content(ISection9Content source)
        {
            Investments = new List<IShortTermInvestment>();
            if (source != null)
            {
                if (source.Investments != null && source.Investments.Any())
                {
                    foreach (IShortTermInvestment _shortTermInv in source.Investments)
                    {
                        Investments.Add(new ShortTermInvestment(_shortTermInv));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section9 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection9Content Content { get; set; }

        public Section9() : base(OUTPUT_SECTION9_CODE)
        {
            Content = new Section9Content();
        }

        public Section9(Section9 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section9Content(source.Content);
            else
                Content = new Section9Content();
        }

    }

}
