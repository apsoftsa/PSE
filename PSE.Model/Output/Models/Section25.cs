using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RelationshipToAdmin : IRelationshipToAdmin
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

        public RelationshipToAdmin()
        {
            Object = null;
            Description = null;
            AddressBook = null;
            Currency = null;
            CurrentBalance = null;
            MarketValueReportingCurrency = null;    
        }

        public RelationshipToAdmin(IRelationshipToAdmin source)
        {
            Object = source.Object;
            Description = source.Description;
            AddressBook = source.AddressBook;
            Currency = source.Currency;
            CurrentBalance = source.CurrentBalance;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section25Content : ISection25Content
    {

        public IList<IRelationshipToAdmin> RelationshipNonTransferedToAdmin { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? TotalAddressBook { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalMarketValueReportingCurrency { get; set; }

        public IList<IRelationshipToAdmin> RelationshipTransferedToAdmin { get; set; }


        public Section25Content()
        {
            RelationshipNonTransferedToAdmin = new List<IRelationshipToAdmin>();
            TotalAddressBook = null;
            TotalMarketValueReportingCurrency = null;
            RelationshipTransferedToAdmin = new List<IRelationshipToAdmin>();
        }

        public Section25Content(ISection25Content source)
        {
            RelationshipNonTransferedToAdmin = new List<IRelationshipToAdmin>();
            TotalAddressBook = null;
            TotalMarketValueReportingCurrency = null;
            RelationshipTransferedToAdmin = new List<IRelationshipToAdmin>();
            if (source != null)
            {
                TotalAddressBook = source.TotalAddressBook;
                TotalMarketValueReportingCurrency = source.TotalMarketValueReportingCurrency;
                if (source.RelationshipNonTransferedToAdmin != null && source.RelationshipNonTransferedToAdmin.Any())
                {
                    foreach (IRelationshipToAdmin _relToAdmin in source.RelationshipNonTransferedToAdmin)
                    {
                        RelationshipNonTransferedToAdmin.Add(new RelationshipToAdmin(_relToAdmin));
                    }
                }
                if (source.RelationshipTransferedToAdmin != null && source.RelationshipTransferedToAdmin.Any())
                {
                    foreach (IRelationshipToAdmin _relToAdmin in source.RelationshipTransferedToAdmin)
                    {
                        RelationshipTransferedToAdmin.Add(new RelationshipToAdmin(_relToAdmin));
                    }
                }
            }

        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section25 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection25Content Content { get; set; }

        public Section25() : base(OUTPUT_SECTION25_CODE)
        {
            Content = new Section25Content();
        }

        public Section25(Section25 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section25Content(source.Content);
            else
                Content = new Section25Content();
        }

    }

}
