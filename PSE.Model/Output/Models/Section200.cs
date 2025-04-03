using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EndExtractCustomer : IEndExtractCustomer
    {

        public string Customer { get; set; }

        public string CustomerID { get; set; }

        public string Portfolio { get; set; }

        public int? RiskProfile { get; set; }

        public string EsgProfile { get; set; }

        public EndExtractCustomer()
        {
            Customer = string.Empty;
            CustomerID = string.Empty;
            Portfolio = string.Empty;
            RiskProfile = 0;
            EsgProfile = string.Empty;
        }

        public EndExtractCustomer(IEndExtractCustomer source)
        {
            Customer = source.Customer;
            CustomerID = source.CustomerID;
            Portfolio = source.Portfolio;
            RiskProfile = source.RiskProfile;
            EsgProfile = source.EsgProfile;
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EndExtractInvestment : IEndExtractInvestment
    {

        public string AssetClass { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentInvestment { get; set; }

        public EndExtractInvestment()
        {
            AssetClass = string.Empty;
            MarketValueReportingCurrency = null;
            PercentInvestment = null;
        }

        public EndExtractInvestment(IEndExtractInvestment source)
        {
            AssetClass = source.AssetClass;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentInvestment = source.PercentInvestment;
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EndExtractInvestmentChart : IEndExtractInvestmentChart
    {

        public string AssetClass { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentInvestment { get; set; }

        public EndExtractInvestmentChart()
        {
            AssetClass = string.Empty;
            PercentInvestment = null;
        }

        public EndExtractInvestmentChart(IEndExtractInvestmentChart source)
        {
            AssetClass = source.AssetClass;
            PercentInvestment = source.PercentInvestment;
        }
    }

    public class SubSection20000 : ISubSection20000
    {

        public string Name { get; set; }

        public IList<IEndExtractCustomer> Content { get; set; }

        public SubSection20000(string name)
        {
            Name = name;
            Content = new List<IEndExtractCustomer>();
        }

        public SubSection20000(ISubSection20000 source)
        {
            Name = source.Name;
            Content = new List<IEndExtractCustomer>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new EndExtractCustomer(item));
            }
        }
    }

    public class SubSection20010 : ISubSection20010
    {

        public string Name { get; set; }

        public IList<IEndExtractInvestment> Content { get; set; }

        public SubSection20010(string name)
        {
            Name = name;
            Content = new List<IEndExtractInvestment>();
        }

        public SubSection20010(ISubSection20010 source)
        {
            Name = source.Name;
            Content = new List<IEndExtractInvestment>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new EndExtractInvestment(item));
            }
        }
    }

    public class SubSection20020 : ISubSection20020
    {

        public string Name { get; set; }

        public IList<IEndExtractInvestmentChart> Content { get; set; }

        public SubSection20020(string name)
        {
            Name = name;
            Content = new List<IEndExtractInvestmentChart>();
        }

        public SubSection20020(ISubSection20020 source)
        {
            Name = source.Name;
            Content = new List<IEndExtractInvestmentChart>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new EndExtractInvestmentChart(item));
            }
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section200Content : ISection200Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection20000? SubSection20000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection20010? SubSection20010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection20020? SubSection20020 { get; set; }

        public Section200Content()
        {
            //SubSection20000 = new SubSection20000(string.Empty);
            //SubSection20010 = new SubSection20010("Investments");
            //SubSection20020 = new SubSection20020("Investments chart");
            SubSection20000 = null;
            SubSection20010 = null;
            SubSection20020 = null;

        }

        public Section200Content(ISection200Content source)
        {
            SubSection20000 = (source.SubSection20000 != null) ? new SubSection20000(source.SubSection20000) : null;
            SubSection20010 = (source.SubSection20010 != null) ? new SubSection20010(source.SubSection20010) : null;
            SubSection20020 = (source.SubSection20020 != null) ? new SubSection20020(source.SubSection20020) : null;
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section200 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection200Content Content { get; set; }

        public Section200() : base()
        {
            Content = new Section200Content();
        }

        public Section200(Section200 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section200Content(source.Content);
            else
                Content = new Section200Content();
        }

    }

}
