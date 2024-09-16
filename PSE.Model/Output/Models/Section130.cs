using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StockOrder : IStockOrder
    {

        [JsonProperty(PropertyName ="order", NullValueHandling = NullValueHandling.Ignore)]
        public string? Order { get; set; }

        [JsonProperty(PropertyName = "value", NullValueHandling = NullValueHandling.Ignore)]
        public long? OrderValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Operation { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Amount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LimitStopLoss { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Price { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ExpirationDate { get; set; }

        public StockOrder()
        {
            Order = string.Empty;
            OrderValue = null;
            Description = null;
            Operation = null;   
            Amount = null;    
            Currency = null;
            LimitStopLoss = string.Empty;
            Price = null;
            ExpirationDate = null;
        }

        public StockOrder(IStockOrder source)
        {
            Order = source.Order;
            OrderValue = source.OrderValue;
            Description = source.Description;
            Operation = source.Operation;
            Amount = source.Amount;
            Currency = source.Currency;
            LimitStopLoss = source.LimitStopLoss;
            Price = source.Price;
            ExpirationDate = source.ExpirationDate;
        }

    }

    public class StockOrderSubSection : IStockOrderSubSection
    {

        public string Name { get; set; }

        public IList<IStockOrder> Content { get; set; }

        public StockOrderSubSection(string name)
        {
            Name = name;
            Content = new List<IStockOrder>();
        }

        public StockOrderSubSection(IStockOrderSubSection source)
        {
            Name = source.Name;
            Content = new List<IStockOrder>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new StockOrder(item));
            }
        }

    }


    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section130Content : ISection130Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IStockOrderSubSection SubSection13000 { get; set; }

        public Section130Content()
        {
            SubSection13000 = new StockOrderSubSection("Stock orders");
        }

        public Section130Content(ISection130Content source)
        {
            SubSection13000 = new StockOrderSubSection(source.SubSection13000);
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section130 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection130Content Content { get; set; }

        public Section130() : base()
        {
            Content = new Section130Content();
        }

        public Section130(Section130 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section130Content(source.Content);
            else
                Content = new Section130Content();
        }

    }

}
