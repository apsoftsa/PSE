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

        public string Description { get; set; }

        public string ValorNumber { get; set; }

        public string Isin { get; set; }

        public string NominalAmmount { get; set; }

        public string Currency { get; set; }

        public string PurchasePrice { get; set; }

        public string PriceBeginningYear { get; set; }

        public string CurrentPriceFromPurchase { get; set; }

        public string CurrentPriceFromYTD { get; set; }

        public string ExchangeRateImpactPurchase { get; set; }

        public string ExchangeRateImpactYTD { get; set; }

        public string PercentPerformancePurchase { get; set; }

        public string PercentPerformanceYTD { get; set; }

        public string PercentAssets { get; set; }

        public AlternativeProductDetail()
        {
            Description = string.Empty;
            ValorNumber = string.Empty;
            Isin = string.Empty;
            NominalAmmount = string.Empty;
            Currency = string.Empty;
            PurchasePrice = string.Empty;
            PriceBeginningYear = string.Empty;
            CurrentPriceFromPurchase = string.Empty;
            CurrentPriceFromYTD = string.Empty;
            ExchangeRateImpactPurchase = string.Empty;
            ExchangeRateImpactYTD = string.Empty;
            PercentPerformancePurchase = string.Empty;
            PercentPerformanceYTD = string.Empty;
            PercentAssets = string.Empty;
        }

        public AlternativeProductDetail(IAlternativeProductDetail source)
        {
            Description = source.Description;
            ValorNumber = source.ValorNumber;
            Isin = source.Isin;
            NominalAmmount = source.NominalAmmount;
            Currency = source.Currency;
            PurchasePrice = source.PurchasePrice;
            PriceBeginningYear = source.PriceBeginningYear;
            CurrentPriceFromPurchase = source.CurrentPriceFromPurchase;
            CurrentPriceFromYTD = source.CurrentPriceFromYTD;
            ExchangeRateImpactPurchase = source.ExchangeRateImpactPurchase;
            ExchangeRateImpactYTD = source.ExchangeRateImpactYTD;
            PercentPerformancePurchase = source.PercentPerformancePurchase;
            PercentPerformanceYTD = source.PercentPerformanceYTD;
            PercentAssets = source.PercentAssets;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AlternativeProducts : IAlternativeProducts
    {

        [JsonIgnore]
        public IList<IAlternativeProductDetail> DerivativesOnSecurities { get; set; }

        [JsonProperty("derivativesOnSecurities")]
        public IList<IAlternativeProductDetail>? SerializationDerivativesOnSecurities
        {
            get { return DerivativesOnSecurities != null && DerivativesOnSecurities.Any() ? (List<IAlternativeProductDetail>)DerivativesOnSecurities : null; }
            private set { }
        }

        [JsonIgnore]
        public IList<IAlternativeProductDetail> DerivativesOnMetals { get; set; }

        [JsonProperty("derivativesOnMetals")]
        public IList<IAlternativeProductDetail>? SerializationDerivativesOnMetals
        {
            get { return DerivativesOnMetals != null && DerivativesOnMetals.Any() ? (List<IAlternativeProductDetail>)DerivativesOnMetals : null; }
            private set { }
        }

        [JsonIgnore]
        public IList<IAlternativeProductDetail> DerivativesFutures { get; set; }

        [JsonProperty("derivativesFutures")]
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

        public AlternativeProducts()
        {
            DerivativesOnSecurities = new List<IAlternativeProductDetail>();
            DerivativesOnMetals = new List<IAlternativeProductDetail>();
            DerivativesFutures = new List<IAlternativeProductDetail>();
            Different = new List<IAlternativeProductDetail>();
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

        public Section18And19() : base(OUTPUT_SECTION18AND19_CODE)
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
