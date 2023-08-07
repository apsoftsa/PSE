using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MetalPhysicalMetalAccount : IMetalPhysicalMetalAccount
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Amount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Account { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CostPrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PurchasingCourse { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentDifference { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CurrentBalance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentAsset { get; set; }

        public MetalPhysicalMetalAccount()
        {
            Amount = null;
            Account = null;
            CostPrice = null;
            PurchasingCourse = null;
            PercentDifference = null;
            CurrentBalance = null;
            MarketValueReportingCurrency = null;
            PercentAsset = null;
        }

        public MetalPhysicalMetalAccount(IMetalPhysicalMetalAccount source)
        {
            Amount = source.Amount;
            Account = source.Account;
            CostPrice = source.CostPrice;
            PurchasingCourse = source.PurchasingCourse;
            PercentDifference = source.PercentDifference;
            CurrentBalance = source.CurrentBalance;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentAsset = source.PercentAsset;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section20Content : ISection20Content
    {

        public IList<IMetalPhysicalMetalAccount> MetalPhysicalMetalAccounts { get; set; }

        public Section20Content()
        {
            MetalPhysicalMetalAccounts = new List<IMetalPhysicalMetalAccount>();
        }

        public Section20Content(ISection20Content source)
        {
            MetalPhysicalMetalAccounts = new List<IMetalPhysicalMetalAccount>();
            if (source != null)
            {
                if (source.MetalPhysicalMetalAccounts != null && source.MetalPhysicalMetalAccounts.Any())
                {
                    foreach (IMetalPhysicalMetalAccount _metPhyMetAcc in source.MetalPhysicalMetalAccounts)
                    {
                        MetalPhysicalMetalAccounts.Add(new MetalPhysicalMetalAccount(_metPhyMetAcc));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section20 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection20Content Content { get; set; }

        public Section20() : base(OUTPUT_SECTION20_CODE)
        {
            Content = new Section20Content();
        }

        public Section20(Section20 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section20Content(source.Content);
            else
                Content = new Section20Content();
        }

    }

}
