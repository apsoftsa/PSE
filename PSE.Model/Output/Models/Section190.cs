using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ObjectReportsNotTransferredToAdministration : IObjectReportsNotTransferredToAdministration
    {

        public string Object { get; set; }

        public string Description { get; set; }

        public string AddressBook { get; set; }

        public string Currency { get; set; }

        public decimal? CurrentBalance { get; set; }

        public decimal? MarketValueReportingCurrency { get; set; }

        public ObjectReportsNotTransferredToAdministration()
        {
            Object = string.Empty;
            Description = string.Empty; 
            AddressBook = string.Empty; 
            Currency = string.Empty;    
            MarketValueReportingCurrency = 0;    
            CurrentBalance = 0;  
        }

        public ObjectReportsNotTransferredToAdministration(IObjectReportsNotTransferredToAdministration source)
        {
            Object = source.Object;
            Description = source.Description;
            AddressBook = source.AddressBook;
            Currency = source.Currency;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            CurrentBalance = source.CurrentBalance;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ObjectReportsTransferredToAdministration : IObjectReportsTransferredToAdministration
    {

        public string Object { get; set; }

        public string Description { get; set; }

        public string AddressBook { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        public decimal? CurrentBalance { get; set; }

        public decimal? MarketValueReportingCurrency { get; set; }

        public ObjectReportsTransferredToAdministration()
        {
            Object = string.Empty;
            Description = string.Empty;
            AddressBook = string.Empty;
            Currency = string.Empty;
            MarketValueReportingCurrency = 0;
            CurrentBalance = 0;
        }

        public ObjectReportsTransferredToAdministration(IObjectReportsTransferredToAdministration source)
        {
            Object = source.Object;
            Description = source.Description;
            AddressBook = source.AddressBook;
            Currency = source.Currency;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            CurrentBalance = source.CurrentBalance;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ReportsTransferredToAdministration : IReportsTransferredToAdministration
    {
     
        public decimal? TotalAsset { get; set; }

        public string TotalAddressBook { get; set; }       

        public decimal? TotalMarketValueReportingCurrency { get; set; }

        public string TotalAssetsNotTransferred { get; set; }

        public decimal? TotalNotTransferredMarketValueReportingCurrency { get; set; }

        public string NotTransferedCount { get; set; }  

        public List<IObjectReportsTransferredToAdministration> Objects { get; set; }

        public ReportsTransferredToAdministration()
        {
            TotalAsset = 0;  
            TotalAddressBook = string.Empty;    
            TotalAssetsNotTransferred = string.Empty;   
            TotalMarketValueReportingCurrency = 0;
            TotalNotTransferredMarketValueReportingCurrency = 0;
            NotTransferedCount = "0";   
            Objects = new List<IObjectReportsTransferredToAdministration>();
        }

        public ReportsTransferredToAdministration(IReportsTransferredToAdministration source)
        {
            TotalAsset = source.TotalAsset;
            TotalAddressBook = source.TotalAddressBook;
            TotalAssetsNotTransferred = source.TotalAssetsNotTransferred;
            TotalMarketValueReportingCurrency = source.TotalMarketValueReportingCurrency;
            TotalNotTransferredMarketValueReportingCurrency = source.TotalNotTransferredMarketValueReportingCurrency;
            NotTransferedCount = source.NotTransferedCount;
            Objects = new List<IObjectReportsTransferredToAdministration>();
            if (source.Objects != null && source.Objects.Any())
            {
                foreach (var item in source.Objects.OrderByDescending(ob => ob.MarketValueReportingCurrency))
                    Objects.Add(new ObjectReportsTransferredToAdministration(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ReportsNotTransferredToAdministration : IReportsNotTransferredToAdministration
    {

        public string TotalAssetsNotTransferred { get; set; }

        public decimal? TotalNotTransferredMarketValueReportingCurrency { get; set; }

        public decimal? TotalAsset { get; set; }

        public decimal? TotalMarketValueReportingCurrency { get; set; }

        public List<IObjectReportsNotTransferredToAdministration> Objects { get; set; }       

        public ReportsNotTransferredToAdministration()
        {
            TotalAsset = 0;
            TotalAssetsNotTransferred = string.Empty;
            TotalMarketValueReportingCurrency = 0;
            TotalNotTransferredMarketValueReportingCurrency = 0;
            Objects = new List<IObjectReportsNotTransferredToAdministration>();
        }

        public ReportsNotTransferredToAdministration(IReportsNotTransferredToAdministration source)
        {
            TotalAsset = source.TotalAsset;
            TotalAssetsNotTransferred = source.TotalAssetsNotTransferred;
            TotalMarketValueReportingCurrency = source.TotalMarketValueReportingCurrency;
            TotalNotTransferredMarketValueReportingCurrency = source.TotalNotTransferredMarketValueReportingCurrency;
            Objects = new List<IObjectReportsNotTransferredToAdministration>();
            if (source.Objects != null && source.Objects.Any())
            {
                foreach (var item in source.Objects.OrderByDescending(ob => ob.MarketValueReportingCurrency))
                    Objects.Add(new ObjectReportsNotTransferredToAdministration(item));
            }
        }

    }

    public class SubSection19000 : ISubSection19000
    {

        public string Name { get; set; }

        public IList<IReportsNotTransferredToAdministration> Content { get; set; }

        public SubSection19000(string name)
        {
            Name = name;
            Content = new List<IReportsNotTransferredToAdministration>();
        }

        public SubSection19000(ISubSection19000 source)
        {
            Name = source.Name;
            Content = new List<IReportsNotTransferredToAdministration>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ReportsNotTransferredToAdministration(item));
            }
        }

    }

    public class SubSection19010 : ISubSection19010
    {

        public string Name { get; set; }

        public IList<IReportsTransferredToAdministration> Content { get; set; }

        public SubSection19010(string name)
        {
            Name = name;
            Content = new List<IReportsTransferredToAdministration>();
        }

        public SubSection19010(ISubSection19010 source)
        {
            Name = source.Name;
            Content = new List<IReportsTransferredToAdministration>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ReportsTransferredToAdministration(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section190Content : ISection190Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection19000? SubSection19000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection19010? SubSection19010 { get; set; }

        public Section190Content()
        {
            SubSection19000 = null;
            SubSection19010 = null;
        }

        public Section190Content(ISection190Content source)
        {
            SubSection19000 = (source.SubSection19000 != null) ? new SubSection19000(source.SubSection19000) : null;
            SubSection19010 = (source.SubSection19010 != null) ? new SubSection19010(source.SubSection19010) : null;
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
