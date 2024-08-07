using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Exchange : IExchange
    {

        [JsonProperty(PropertyName ="order", NullValueHandling = NullValueHandling.Ignore)]
        public string ExchangeOrder { get; set; }

        [JsonProperty(PropertyName = "value", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExchangeValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Operation { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Quantity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(PropertyName = "limiteStop-loss", NullValueHandling = NullValueHandling.Ignore)]
        public string LimitStopLoss { get; set; }

        [JsonProperty(PropertyName = "course-cost", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CourseCost { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ExpirationDate { get; set; }

        public Exchange()
        {
            ExchangeOrder = string.Empty;
            ExchangeValue = null;
            Description = null;
            Operation = null;   
            Quantity = null;    
            Currency = null;
            LimitStopLoss = string.Empty;
            CourseCost = null;
            ExpirationDate = null;
        }

        public Exchange(IExchange source)
        {
            ExchangeOrder = source.ExchangeOrder;
            ExchangeValue = source.ExchangeValue;
            Description = source.Description;
            Operation = source.Operation;
            Quantity = source.Quantity;
            Currency = source.Currency;
            LimitStopLoss = source.LimitStopLoss;
            CourseCost = source.CourseCost;
            ExpirationDate = source.ExpirationDate;
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section24Content : ISection24Content
    {

        public IList<IExchange> Exchange { get; set; }

        public Section24Content()
        {
            Exchange = new List<IExchange>();
        }

        public Section24Content(ISection24Content source)
        {
            Exchange = new List<IExchange>();
            if (source != null)
            {
                if (source.Exchange != null && source.Exchange.Any())
                {
                    foreach (IExchange exch in source.Exchange)
                    {
                        Exchange.Add(new Exchange(exch));
                    }
                }

            }

        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section24 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection24Content Content { get; set; }

        public Section24() : base()
        {
            Content = new Section24Content();
        }

        public Section24(Section24 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section24Content(source.Content);
            else
                Content = new Section24Content();
        }

    }

}
