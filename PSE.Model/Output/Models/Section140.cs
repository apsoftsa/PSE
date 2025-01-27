using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FundAccumulationPlanPayment : IFundAccumulationPlanPayment
    {

        public string Frequency { get; set; }

        public string Currency { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Executed { get; set; }

        public FundAccumulationPlanPayment()
        {
            Frequency = string.Empty;
            Currency = string.Empty;
            Amount = 0;
            Executed = 0;
        }

        public FundAccumulationPlanPayment(IFundAccumulationPlanPayment source)
        {
            Frequency = source.Frequency;
            Currency = source.Currency;
            Amount = source.Amount;
            Executed = source.Executed;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FundAccumulationPlan : IFundAccumulationPlan
    {

        public string ExpirationDate { get; set; }

        public string Description1 { get; set; }

        public string Description2 { get; set; }

        public IList<IFundAccumulationPlanPayment> Payments { get; set; }

        public decimal? AveragePurchasePrice { get; set; }

        public decimal? SharesPurchased { get; set; }

        public decimal? Exchange { get; set; }

        public decimal? MarketValueReportingCurrency { get; set; }

        public decimal? PercentWeigth { get; set; }

        public FundAccumulationPlan()
        {
            Description1 = string.Empty;
            Description2 = string.Empty;    
            AveragePurchasePrice = 0;   
            SharesPurchased = 0;
            Exchange = 0;
            MarketValueReportingCurrency = 0;
            PercentWeigth = 0;  
            ExpirationDate = string.Empty;
            Payments = new List<IFundAccumulationPlanPayment>();    
        }

        public FundAccumulationPlan(IFundAccumulationPlan source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            ExpirationDate = source.ExpirationDate;
            AveragePurchasePrice = source.AveragePurchasePrice;
            SharesPurchased = source.SharesPurchased;
            Exchange = source.Exchange;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentWeigth = source.PercentWeigth;
            Payments = new List<IFundAccumulationPlanPayment>();
            if (source.Payments != null && source.Payments.Any())
            {
                foreach (var item in source.Payments)
                    Payments.Add(new FundAccumulationPlanPayment(item));
            }
        }

    }

    public class FACSubSection : IFACSubSection
    {

        public string Name { get; set; }

        public IList<IFundAccumulationPlan> Content { get; set; }

        public FACSubSection(string name)
        {
            Name = name;
            Content = new List<IFundAccumulationPlan>();
        }

        public FACSubSection(IFACSubSection source)
        {
            Name = source.Name;
            Content = new List<IFundAccumulationPlan>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new FundAccumulationPlan(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section140Content : ISection140Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IFACSubSection SubSection14000 { get; set; }

        public Section140Content()
        {
            SubSection14000 = new FACSubSection("Funds accumulation plan");
        }

        public Section140Content(ISection140Content source)
        {
            SubSection14000 = new FACSubSection(source.SubSection14000);
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section140 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection140Content Content { get; set; }

        public Section140() : base()
        {
            Content = new Section140Content();
        }

        public Section140(Section140 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section140Content(source.Content);
            else
                Content = new Section140Content();
        }

    }

}
