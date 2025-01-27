using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FinancialSolutionMortgage : IFinancialSolutionMortgage
    {

        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public decimal? PercentRate { get; set; }
        public string OpeningDate { get; set; }
        public string ExpirationDate { get; set; }
        public string CurrentBalance { get; set; }
        public decimal? MarketValueReportingCurrency { get; set; }
        public decimal? AccruedInterestReportingCurrency { get; set; }
        public decimal? Exchange { get; set; }

        public FinancialSolutionMortgage()
        {
            Description1 = string.Empty;    
            Description2 = string.Empty;    
            PercentRate = 0;
            OpeningDate = string.Empty;
            ExpirationDate = string.Empty;
            CurrentBalance = string.Empty;  
            MarketValueReportingCurrency = 0;
            AccruedInterestReportingCurrency = 0;
            Exchange = 0;       
        }

        public FinancialSolutionMortgage(IFinancialSolutionMortgage source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            PercentRate = source.PercentRate;
            OpeningDate = source.OpeningDate;
            ExpirationDate = source.ExpirationDate;
            CurrentBalance = source.CurrentBalance;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            AccruedInterestReportingCurrency = source.AccruedInterestReportingCurrency;
            Exchange = source.Exchange;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FinancialSolutionAccount : IFinancialSolutionAccount
    {             

        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public decimal? AdvanceValue { get; set; }
        public decimal? QuantityUsed { get; set; }
        public decimal? QuantityAvailable { get; set; }
        public decimal? PercentRate { get; set; }
        public string OpeningDate { get; set; }
        public decimal? MarketValueReportingCurrency { get; set; }
        public decimal? Exchange { get; set; }

        public FinancialSolutionAccount()
        {
            Description1 = string.Empty;
            Description2 = string.Empty;
            AdvanceValue = 0;   
            QuantityUsed = 0;
            QuantityAvailable = 0;  
            PercentRate = 0;
            OpeningDate = string.Empty;
            MarketValueReportingCurrency = 0;
            Exchange = 0;
        }

        public FinancialSolutionAccount(IFinancialSolutionAccount source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            PercentRate = source.PercentRate;
            OpeningDate = source.OpeningDate;
            AdvanceValue=source.AdvanceValue;
            QuantityAvailable=source.QuantityAvailable;
            QuantityUsed=source.QuantityUsed;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;            
            Exchange = source.Exchange;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FinancialSolutionFixed : FinancialSolutionMortgage, IFinancialSolutionFixed
    {

        public decimal? PercentWeight { get; set; }

        public FinancialSolutionFixed()
        {
            PercentWeight = 0;
        }

        public FinancialSolutionFixed(IFinancialSolutionFixed source)
        {
            PercentWeight = source.PercentWeight;
        }

    }

    public class SubSection12000 : ISubSection12000
    {
        public string Name { get; set; }
        public IList<IFinancialSolutionMortgage> Content { get; set; }

        public SubSection12000(string name)
        {
            Name = name;
            Content = new List<IFinancialSolutionMortgage>();
        }

        public SubSection12000(ISubSection12000 source)
        {
            Name = source.Name;
            Content = new List<IFinancialSolutionMortgage>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new FinancialSolutionMortgage(item));
            }
        }

    }

    public class SubSection12010 : ISubSection12010
    {
        public string Name { get; set; }
        public IList<IFinancialSolutionAccount> Content { get; set; }

        public SubSection12010(string name)
        {
            Name = name;
            Content = new List<IFinancialSolutionAccount>();
        }

        public SubSection12010(ISubSection12010 source)
        {
            Name = source.Name;
            Content = new List<IFinancialSolutionAccount>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new FinancialSolutionAccount(item));
            }
        }

    }

    public class SubSection12020 : ISubSection12020
    {
        public string Name { get; set; }
        public IList<IFinancialSolutionFixed>? Content { get; set; }

        public SubSection12020(string name)
        {
            Name = name;
            Content = new List<IFinancialSolutionFixed>();
        }

        public SubSection12020(ISubSection12020 source)
        {
            Name = source.Name;
            Content = new List<IFinancialSolutionFixed>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new FinancialSolutionFixed(item));
            }
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section120Content : ISection120Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection12000? SubSection12000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection12010? SubSection12010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection12020? SubSection12020 { get; set; }

        public Section120Content()
        {
            /*
            SubSection12000 = new SubSection12000("Mortgage and construction loans");
            SubSection12010 = new SubSection12010("Current account advances");
            SubSection12020 = new SubSection12020("Fixed Advances");
            */
            SubSection12000 = null;
            SubSection12010 = null;
            SubSection12020 = null;
        }

        public Section120Content(ISection120Content source)
        {
            SubSection12000 = (source.SubSection12000 != null) ? new SubSection12000(source.SubSection12000) : null;
            SubSection12010 = (source.SubSection12010 != null) ? new SubSection12010(source.SubSection12010) : null;
            SubSection12020 = (source.SubSection12020 != null) ? new SubSection12020(source.SubSection12020) : null;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section120 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection120Content Content { get; set; }

        public Section120() : base()
        {
            Content = new Section120Content();
        }

        public Section120(Section120 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section120Content(source.Content);
            else
                Content = new Section120Content();
        }

    }

}
