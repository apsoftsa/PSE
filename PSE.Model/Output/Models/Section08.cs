using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Account : IAccount
    {

        [JsonProperty(propertyName: "account", NullValueHandling = NullValueHandling.Ignore)]
        public string? AccountData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Iban { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CurrentBalance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? AccruedInterestReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentAssets { get; set; }

        public Account()
        {
            this.AccountData = null;
            this.Iban = null;
            this.Currency = null;
            this.CurrentBalance = null;
            this.MarketValueReportingCurrency = null;
            this.AccruedInterestReportingCurrency = null;
            this.PercentAssets = null;
        }

        public Account(IAccount source)
        {
            this.AccountData = source.AccountData;
            this.Iban = source.Iban;
            this.Currency = source.Currency;
            this.CurrentBalance = source.CurrentBalance;
            this.MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            this.AccruedInterestReportingCurrency = source.AccruedInterestReportingCurrency;
            this.PercentAssets = source.PercentAssets;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section8Content : ISection8Content
    {

        [JsonProperty(propertyName: "accounts")]
        public IList<IAccount> Accounts { get; set; }

        public Section8Content()
        {
            Accounts = new List<IAccount>();
        }

        public Section8Content(ISection8Content source)
        {
            Accounts = new List<IAccount>();
            if (source != null)
            {
                if (source.Accounts != null && source.Accounts.Any())
                {
                    foreach (IAccount _account in source.Accounts)
                    {
                        Accounts.Add(new Account(_account));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section8 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection8Content Content { get; set; }

        public Section8() : base(OUTPUT_SECTION8_CODE)
        {
            Content = new Section8Content();
        }

        public Section8(Section8 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section8Content(source.Content);
            else
                Content = new Section8Content();
        }

    }

}
