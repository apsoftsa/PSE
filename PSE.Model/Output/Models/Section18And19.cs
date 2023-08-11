using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AlternativeProductDetail : IAlternativeProductDetail
    {

        public string? Description { get; set; }

        public string? DescriptionExtra { get; set; }

        public long? ValorNumber { get; set; }

        public string? Isin { get; set; }

        public decimal? NominalAmount { get; set; }

        public string? UnderlyingDescription { get; set; }

        public string? Currency { get; set; }

        public decimal? PurchasePrice { get; set; }

        public decimal? PriceBeginningYear { get; set; }

        public decimal? CurrentPrice { get; set; }

        public decimal? ExchangeRateImpactPurchase { get; set; }

        public decimal? ExchangeRateImpactYTD { get; set; }

        public decimal? PerformancePurchase { get; set; }

        public decimal? PercentPerformancePurchase { get; set; }

        public decimal? PerformanceYTD { get; set; }

        public decimal? PercentPerformanceYTD { get; set; }

        public decimal? PercentAsset { get; set; }

        public AlternativeProductDetail()
        {
            Description = null;
            DescriptionExtra = null;    
            ValorNumber = null;
            Isin = null;
            NominalAmount = null;
            UnderlyingDescription = null;   
            Currency = null;
            PurchasePrice = null;
            PriceBeginningYear = null;
            CurrentPrice = null;
            ExchangeRateImpactPurchase = null;
            ExchangeRateImpactYTD = null;
            PerformancePurchase = null; 
            PercentPerformancePurchase = null;
            PerformanceYTD = null;  
            PercentPerformanceYTD = null;
            PercentAsset = null;
        }

        public AlternativeProductDetail(IAlternativeProductDetail source)
        {
            Description = source.Description;
            DescriptionExtra = source.DescriptionExtra;
            ValorNumber = source.ValorNumber;
            Isin = source.Isin;
            NominalAmount = source.NominalAmount;
            UnderlyingDescription = source.UnderlyingDescription;
            Currency = source.Currency;
            PurchasePrice = source.PurchasePrice;
            PriceBeginningYear = source.PriceBeginningYear;
            CurrentPrice = source.CurrentPrice;
            ExchangeRateImpactPurchase = source.ExchangeRateImpactPurchase;
            ExchangeRateImpactYTD = source.ExchangeRateImpactYTD;
            PerformancePurchase = source.PerformancePurchase;   
            PercentPerformancePurchase = source.PercentPerformancePurchase;
            PerformanceYTD = source.PercentAsset;   
            PercentPerformanceYTD = source.PercentPerformanceYTD;
            PercentAsset = source.PercentAsset;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AlternativeProducts : IAlternativeProducts
    {

        [JsonIgnore]
        public IList<IAlternativeProductDetail> DerivativesOnSecurities { get; set; }

        [JsonProperty("derivativesOnSecurity")]
        public IList<IAlternativeProductDetail>? SerializationDerivativesOnSecurities
        {
            get { return DerivativesOnSecurities != null && DerivativesOnSecurities.Any() ? (List<IAlternativeProductDetail>)DerivativesOnSecurities : null; }
            private set { }
        }

        [JsonIgnore]
        public IList<IAlternativeProductDetail> DerivativesOnMetals { get; set; }

        [JsonProperty("derivativesOnMetal")]
        public IList<IAlternativeProductDetail>? SerializationDerivativesOnMetals
        {
            get { return DerivativesOnMetals != null && DerivativesOnMetals.Any() ? (List<IAlternativeProductDetail>)DerivativesOnMetals : null; }
            private set { }
        }

        [JsonIgnore]
        public IList<IAlternativeProductDetail> DerivativesFutures { get; set; }

        [JsonProperty("derivativesFuture")]
        public IList<IAlternativeProductDetail>? SerializationDerivativesFutures
        {
            get { return DerivativesFutures != null && DerivativesFutures.Any() ? (List<IAlternativeProductDetail>)DerivativesFutures : null; }
            private set { }
        }

        [JsonIgnore]
        public IList<IAlternativeProductDetail> Different { get; set; }

        [JsonProperty("different")]
        public IList<IAlternativeProductDetail>? SerializationDifferent
        {
            get { return Different != null && Different.Any() ? (List<IAlternativeProductDetail>)Different : null; }
            private set { }
        }

        [JsonIgnore]
        public IList<IAlternativeProductDetail> DifferentExtra { get; set; }

        [JsonProperty("differentExtra")]
        public IList<IAlternativeProductDetail>? SerializationDifferentExtra
        {
            get { return DifferentExtra != null && DifferentExtra.Any() ? (List<IAlternativeProductDetail>)DifferentExtra : null; }
            private set { }
        }

        public AlternativeProducts()
        {
            DerivativesOnSecurities = new List<IAlternativeProductDetail>();
            DerivativesOnMetals = new List<IAlternativeProductDetail>();
            DerivativesFutures = new List<IAlternativeProductDetail>();
            Different = new List<IAlternativeProductDetail>();
            DifferentExtra = new List<IAlternativeProductDetail>();
        }

        public AlternativeProducts(IAlternativeProducts source)
        {
            DerivativesOnSecurities = new List<IAlternativeProductDetail>();
            if (source.DerivativesOnSecurities != null && source.DerivativesOnSecurities.Any())
            {
                foreach (IAlternativeProductDetail _altProdDet in source.DerivativesOnSecurities)
                {
                    DerivativesOnSecurities.Add(new AlternativeProductDetail(_altProdDet));
                }
            }
            DerivativesOnMetals = new List<IAlternativeProductDetail>();
            if (source.DerivativesOnMetals != null && source.DerivativesOnMetals.Any())
            {
                foreach (IAlternativeProductDetail _altProdDet in source.DerivativesOnMetals)
                {
                    DerivativesOnMetals.Add(new AlternativeProductDetail(_altProdDet));
                }
            }
            DerivativesFutures = new List<IAlternativeProductDetail>();
            if (source.DerivativesFutures != null && source.DerivativesFutures.Any())
            {
                foreach (IAlternativeProductDetail _altProdDet in source.DerivativesFutures)
                {
                    DerivativesFutures.Add(new AlternativeProductDetail(_altProdDet));
                }
            }
            Different = new List<IAlternativeProductDetail>();
            if (source.Different != null && source.Different.Any())
            {
                foreach (IAlternativeProductDetail _altProdDet in source.Different)
                {
                    Different.Add(new AlternativeProductDetail(_altProdDet));
                }
            }
            DifferentExtra = new List<IAlternativeProductDetail>();
            if (source.DifferentExtra != null && source.DifferentExtra.Any())
            {
                foreach (IAlternativeProductDetail _altProdDet in source.DifferentExtra)
                {
                    DifferentExtra.Add(new AlternativeProductDetail(_altProdDet));
                }
            }
        }

    }     

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section18And19Content : ISection18And19Content
    {

        [JsonProperty(propertyName: "alternativeProducts", Order = 1)]
        public IList<IAlternativeProducts> AlternativeProducts { get; set; }

        public Section18And19Content()
        {
            AlternativeProducts = new List<IAlternativeProducts>();
        }

        public Section18And19Content(ISection18And19Content source)
        {
            AlternativeProducts = new List<IAlternativeProducts>();
            if (source != null)
            {
                if (source.AlternativeProducts != null && source.AlternativeProducts.Any())
                {
                    foreach (IAlternativeProducts _altProd in source.AlternativeProducts)
                    {
                        AlternativeProducts.Add(new AlternativeProducts(_altProd));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section18And19 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection18And19Content Content { get; set; }

        public Section18And19() : base()
        {
            Content = new Section18And19Content();
        }

        public Section18And19(Section18And19 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section18And19Content(source.Content);
            else
                Content = new Section18And19Content();
        }

    }

}
