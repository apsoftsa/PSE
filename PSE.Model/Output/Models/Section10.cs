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
        public string Account { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NoDeposit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PercentInterest { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Correspondent { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OpeningDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExpirationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FaceValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AccruedInterestReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PercentAssets { get; set; }

        public FiduciaryInvestmentAccount()
        {
            Account = string.Empty;
            Currency = string.Empty;
            NoDeposit = string.Empty;
            PercentInterest = string.Empty;
            Correspondent = string.Empty;
            OpeningDate = string.Empty;
            ExpirationDate = string.Empty;
            FaceValue = string.Empty;
            MarketValueReportingCurrency = string.Empty;
            AccruedInterestReportingCurrency = string.Empty;
            PercentAssets = string.Empty;
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
            PercentAssets = source.PercentAssets;
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
