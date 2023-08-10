using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PossibleCommitment : IPossibleCommitment
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Account { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? OpeningDay { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ExpirationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CurrentBalance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? AccruedInterestReportingCurrency { get; set; }

        public PossibleCommitment()
        {
            Account = null;
            OpeningDay = null;
            ExpirationDate = null;
            CurrentBalance = null;
            MarketValueReportingCurrency = null;
            AccruedInterestReportingCurrency = null;
        }

        public PossibleCommitment(IPossibleCommitment source)
        {
            Account = source.Account;
            OpeningDay = source.OpeningDay;
            ExpirationDate = source.ExpirationDate;
            CurrentBalance = source.CurrentBalance;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            AccruedInterestReportingCurrency = source.AccruedInterestReportingCurrency;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MortgageLoan : IMortgageLoan
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Account { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? OpeningDay { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ExpirationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Rate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CurrentBalance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? AccruedInterestReportingCurrency { get; set; }

        public MortgageLoan()
        {
            Account = null;
            Currency = null;
            OpeningDay = null;
            ExpirationDate = null;
            Rate = null;
            CurrentBalance = null;
            MarketValueReportingCurrency = null;
            AccruedInterestReportingCurrency = null;
        }

        public MortgageLoan(IMortgageLoan source)
        {
            Account = source.Account;
            Currency = source.Currency;
            OpeningDay = source.OpeningDay;
            ExpirationDate = source.ExpirationDate;
            Rate = source.Rate;
            CurrentBalance = source.CurrentBalance;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            AccruedInterestReportingCurrency = source.AccruedInterestReportingCurrency;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section21Content : ISection21Content
    {

        public IList<IPossibleCommitment> PossibleCommitments { get; set; }

        public IList<IMortgageLoan> MortgageLoans { get; set; }


        public Section21Content()
        {
            PossibleCommitments = new List<IPossibleCommitment>();
            MortgageLoans = new List<IMortgageLoan>();
        }

        public Section21Content(ISection21Content source)
        {
            PossibleCommitments = new List<IPossibleCommitment>();
            MortgageLoans = new List<IMortgageLoan>();
            if (source != null)
            {
                if (source.PossibleCommitments != null && source.PossibleCommitments.Any())
                {
                    foreach (IPossibleCommitment _possComm in source.PossibleCommitments)
                    {
                        PossibleCommitments.Add(new PossibleCommitment(_possComm));
                    }
                }
                if (source.MortgageLoans != null && source.MortgageLoans.Any())
                {
                    foreach (IMortgageLoan _mortLoan in source.MortgageLoans)
                    {
                        MortgageLoans.Add(new MortgageLoan(_mortLoan));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section21 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection21Content Content { get; set; }

        public Section21() : base()
        {
            Content = new Section21Content();
        }

        public Section21(Section21 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section21Content(source.Content);
            else
                Content = new Section21Content();
        }

    }

}
