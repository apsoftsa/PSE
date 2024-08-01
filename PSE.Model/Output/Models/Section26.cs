using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class OverviewPortfolio : IOverviewPortfolio
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? CustomerName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? CustomerNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Portfolio { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Service { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? RiskProfile { get; set; }

        public OverviewPortfolio()
        {
            CustomerName = null;
            CustomerNumber = null;
            Portfolio = null;
            Service = null;
            RiskProfile = null;
        }

        public OverviewPortfolio(IOverviewPortfolio source)
        {
            CustomerName = source.CustomerName;
            CustomerNumber = source.CustomerNumber;
            Portfolio = source.Portfolio;
            Service = source.Service;
            RiskProfile = source.RiskProfile;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GraphicalInvestment : IGraphicalInvestment
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? TypeInvestment { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Percent { get; set; }

        public GraphicalInvestment()
        {
            TypeInvestment = null;
            Percent = null;
        }

        public GraphicalInvestment(IGraphicalInvestment source)
        {
            TypeInvestment = source.TypeInvestment;
            Percent = source.Percent;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class TypeInvestment : ITypeInvestment
    {

        [JsonProperty(PropertyName = "typeInvestment", NullValueHandling = NullValueHandling.Ignore)]
        public string? InvestmentType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        public TypeInvestment()
        {
            InvestmentType = null;
            MarketValueReportingCurrency = null;
        }

        public TypeInvestment(ITypeInvestment source)
        {
            InvestmentType = source.InvestmentType;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AnyCommitments : IAnyCommitments
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        public AnyCommitments()
        {
            MarketValueReportingCurrency = null;
        }

        public AnyCommitments(IAnyCommitments source)
        {
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section26Content : ISection26Content
    {

        public IList<IOverviewPortfolio> OverviewPortfolio { get; set; }

        public IList<IGraphicalInvestment> ChartGraphicalInvestment { get; set; }

        public IList<ITypeInvestment> Investment { get; set; }

        public IList<IAnyCommitments> InformationPosition { get; set; }

        public Section26Content()
        {
            OverviewPortfolio = new List<IOverviewPortfolio>();
            ChartGraphicalInvestment = new List<IGraphicalInvestment>();
            Investment = new List<ITypeInvestment>();
            InformationPosition = new List<IAnyCommitments>();
        }

        public Section26Content(ISection26Content source)
        {
            OverviewPortfolio = new List<IOverviewPortfolio>();
            ChartGraphicalInvestment = new List<IGraphicalInvestment>();
            Investment = new List<ITypeInvestment>();
            InformationPosition = new List<IAnyCommitments>();
            if (source != null)
            {
                if (source.OverviewPortfolio != null && source.OverviewPortfolio.Any())
                {
                    foreach (IOverviewPortfolio overPort in source.OverviewPortfolio)
                    {
                        OverviewPortfolio.Add(new OverviewPortfolio(overPort));
                    }
                }
                if (source.ChartGraphicalInvestment != null && source.ChartGraphicalInvestment.Any())
                {
                    foreach (IGraphicalInvestment graphInvest in source.ChartGraphicalInvestment)
                    {
                        ChartGraphicalInvestment.Add(new GraphicalInvestment(graphInvest));
                    }
                }
                if (source.Investment != null && source.Investment.Any())
                {
                    foreach (ITypeInvestment typeInvestment in source.Investment)
                    {
                        Investment.Add(new TypeInvestment(typeInvestment));
                    }
                }
                if (source.InformationPosition != null && source.InformationPosition.Any())
                {
                    foreach (IAnyCommitments anyComm in source.InformationPosition)
                    {
                        InformationPosition.Add(new AnyCommitments(anyComm));
                    }
                }
            }

        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section26 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection26Content Content { get; set; }

        public Section26() : base()
        {
            Content = new Section26Content();
        }

        public Section26(Section26 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section26Content(source.Content);
            else
                Content = new Section26Content();
        }

    }

}
