using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{


    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FiduciaryInvestmentAccount : IFiduciaryInvestmentAccount
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Account { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? NoDeposit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentInterest { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Correspondent { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? OpeningDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ExpirationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? FaceValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? AccruedInterestReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentAsset { get; set; }
        
        public FiduciaryInvestmentAccount()
        {
            Account = null;
            Currency = null;
            NoDeposit = null;
            PercentInterest = null;
            Correspondent = null;
            OpeningDate = null;
            ExpirationDate = null;
            FaceValue = null;
            MarketValueReportingCurrency = null;
            AccruedInterestReportingCurrency = null;
            PercentAsset = null;
        }

        public FiduciaryInvestmentAccount(IFiduciaryInvestmentAccount source)
        {
            Account = source.Account;
            Currency = source.Currency;
            NoDeposit = source.NoDeposit;
            PercentInterest = source.PercentInterest;
            Correspondent = source.Correspondent;
            OpeningDate = source.OpeningDate;
            ExpirationDate = source.ExpirationDate;
            FaceValue = source.FaceValue;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            AccruedInterestReportingCurrency = source.AccruedInterestReportingCurrency;
            PercentAsset = source.PercentAsset;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section10Content : ISection10Content
    {

        public IList<IFiduciaryInvestmentAccount> Accounts { get; set; }

        public Section10Content()
        {
            Accounts = new List<IFiduciaryInvestmentAccount>();
        }

        public Section10Content(ISection10Content source)
        {
            Accounts = new List<IFiduciaryInvestmentAccount>();
            if (source != null)
            {
                if (source.Accounts != null && source.Accounts.Any())
                {
                    foreach (IFiduciaryInvestmentAccount _fidInvAcc in source.Accounts)
                    {
                        Accounts.Add(new FiduciaryInvestmentAccount(_fidInvAcc));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section10 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection10Content Content { get; set; }

        public Section10() : base(OUTPUT_SECTION10_CODE)
        {
            Content = new Section10Content();
        }

        public Section10(Section10 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section10Content(source.Content);
            else
                Content = new Section10Content();
        }

    }

}
