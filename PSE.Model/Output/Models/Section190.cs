using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AccountAndDepositReport : IAccountAndDepositReport
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Object { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? AddressBook { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CurrentBalance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Total { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? TotalAsset { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? TotalAddressBook { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? TotalAssetsNotTransferred { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalMarketValueReportingCurrency { get; set; }

        public AccountAndDepositReport()
        {
            Object = null;
            Description = null;
            AddressBook = null;
            Currency = null;
            CurrentBalance = null;
            MarketValueReportingCurrency = null;
            Total = null;
            TotalAsset = null;  
            TotalAddressBook = null;    
            TotalAssetsNotTransferred = null;   
            TotalMarketValueReportingCurrency = null;   
        }

        public AccountAndDepositReport(IAccountAndDepositReport source)
        {
            Object = source.Object;
            Description = source.Description;
            AddressBook = source.AddressBook;
            Currency = source.Currency;
            CurrentBalance = source.CurrentBalance;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            Total = source.Total;
            TotalAsset = source.TotalAsset;
            TotalAddressBook = source.TotalAddressBook;
            TotalAssetsNotTransferred = source.TotalAssetsNotTransferred;
            TotalMarketValueReportingCurrency = source.TotalMarketValueReportingCurrency;
        }

    }

    public class SubSection19000 : ISubSection19000
    {

        public string? Name { get; set; }

        public IList<IAccountAndDepositReport>? Content { get; set; }

        public SubSection19000(string name)
        {
            Name = name;
            Content = new List<IAccountAndDepositReport>();
        }

        public SubSection19000(ISubSection19000 source)
        {
            Name = source.Name;
            Content = new List<IAccountAndDepositReport>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new AccountAndDepositReport(item));
            }
        }

    }

    public class SubSection19010 : ISubSection19010
    {

        public string? Name { get; set; }

        public IList<IAccountAndDepositReport>? Content { get; set; }

        public SubSection19010(string name)
        {
            Name = name;
            Content = new List<IAccountAndDepositReport>();
        }

        public SubSection19010(ISubSection19010 source)
        {
            Name = source.Name;
            Content = new List<IAccountAndDepositReport>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new AccountAndDepositReport(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section190Content : ISection190Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection19000 SubSection19000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection19010 SubSection19010 { get; set; }

        public Section190Content()
        {
            SubSection19000 = new SubSection19000("Reports not transferred to administration");
            SubSection19010 = new SubSection19010("Reports transferred to administration");
        }

        public Section190Content(ISection190Content source)
        {
            SubSection19000 = new SubSection19000(source.SubSection19000);
            SubSection19010 = new SubSection19010(source.SubSection19010);
        }

    }


    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section190 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection190Content Content { get; set; }

        public Section190() : base()
        {
            Content = new Section190Content();
        }

        public Section190(Section190 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section190Content(source.Content);
            else
                Content = new Section190Content();
        }

    }

}
